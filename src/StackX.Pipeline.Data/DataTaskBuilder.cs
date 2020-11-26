using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace StackX.Pipeline.Data
{
    public class DataTaskBuilder
    {
        protected IDbConnection? _connection;
        
        public DataTaskBuilder SetConnection(IDbConnection connection)
        {
            _connection = connection;
            return this;
        }

        public DataTaskBuilderRead<TTable,TArgs> Read<TTable,TArgs>()
        {
            return new(_connection);
        } 
        
        internal DataTaskBuilder () {}

        public static DataTaskBuilder New() => new();

    }

    public record QueryBuilderArgs<TQuery, TArgs>(SqlExpression<TQuery> Expression, TArgs PipeArgs);
    
    public class DataTaskBuilderRead<TTable,TArgs> : DataTaskBuilder
    {
        private Func<QueryBuilderArgs<TTable,TArgs>, SqlExpression<TTable>> _queryBuilder;
        private string? _onEmptyRaiseError = null;
        
        internal DataTaskBuilderRead(IDbConnection connection)
        {
            _connection = connection;
        }

        public DataTaskBuilderRead<TTable,TArgs> Query(Func<QueryBuilderArgs<TTable,TArgs>, SqlExpression<TTable>> builder)
        {
            _queryBuilder = builder;
            return this;
        }

        public DataTaskBuilderRead<TTable, TArgs> OnEmptyRaiseError(string message = "no results found")
        {
            _onEmptyRaiseError = message;
            return this;
        }


        public PipeElement Build()
        {
            return new DataQueryElement<TTable, TArgs>(_connection, _queryBuilder, _onEmptyRaiseError);
        }
    }


    internal class DataQueryElement<TTable, TArgs> : PipeElement<TArgs>
    {
        private IDbConnection? _connection;
        private readonly Func<QueryBuilderArgs<TTable, TArgs>, SqlExpression<TTable>> _builder;
        private readonly string? _onEmptyRaiseError;

        internal DataQueryElement(IDbConnection? connection,
            Func<QueryBuilderArgs<TTable, TArgs>, SqlExpression<TTable>> builder, string? onEmptyRaiseError)
        {
            _connection = connection;
            _builder = builder;
            _onEmptyRaiseError = onEmptyRaiseError;
        }

        private IDbConnection Db
        {
            get
            {
                return _connection ??= HostContext.AppHost.GetDbConnection();
            }
        }
        
        protected override PipeElementResult Execute(TArgs args, PipelineState state)
        {
            var expression = _builder(new QueryBuilderArgs<TTable, TArgs>(Db.From<TTable>(), args));

            var result = Db.Select(expression);

            if (_onEmptyRaiseError.IsNullOrEmpty() is false)
            {
                return new PipeErrorResult {ErrorObject = _onEmptyRaiseError};
            }
            
            return new PipeSuccessResult
            {
                Result = result
            };
        }
    }
}