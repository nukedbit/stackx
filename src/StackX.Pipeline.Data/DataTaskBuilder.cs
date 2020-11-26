using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StackX.Pipeline.Data
{
    public record QueryBuilderArgs<TQuery, TArgs>(SqlExpression<TQuery> Expression, TArgs PipeArgs);

    internal record QuerySqlSelect(string sql, object? anonType);

    internal enum SelectType
    {
        Single,
        List
    }
    
    public class DataTaskBuilder
    {
        protected IDbConnection? _connection;
        
        public DataTaskBuilder SetConnection(IDbConnection connection)
        {
            _connection = connection;
            return this;
        }

        public IReadQueryBuilder<TTable,TArgs> Read<TTable,TArgs>()
        {
            return new DataTaskBuilderRead<TTable,TArgs>(_connection);
        } 
        
        internal DataTaskBuilder () {}

        public static DataTaskBuilder New() => new();

    }

    public interface IReadQueryBuilder<TTable, TArgs>
    {
        public IReadQueryModifiers<TTable, TArgs> Query(
            Func<QueryBuilderArgs<TTable, TArgs>, SqlExpression<TTable>> builder);

        public IReadQueryModifiers<TTable, TArgs> Query(string sql);

        public IReadQueryModifiers<TTable, TArgs> Query(string sql, object anonType);
    }

    public interface IReadQueryModifiers<TTable, TArgs>
    {
        public IReadQueryModifiers<TTable, TArgs> OnEmptyOrNullRaiseError(string message = "no results found");
        
        public IQueryPipeBuilder AsList();

        public IQueryPipeBuilder AsSingle();
    }
    
    public interface IQueryPipeBuilder
    {
        PipeElement Build();
    }

    internal class DataTaskBuilderRead<TTable,TArgs> : DataTaskBuilder, IReadQueryBuilder<TTable, TArgs>, IReadQueryModifiers<TTable, TArgs>, IQueryPipeBuilder
    {
        private Func<QueryBuilderArgs<TTable,TArgs>, SqlExpression<TTable>> _queryBuilder;
        private string? _onEmptyOrNullRaiseError = null;
        private QuerySqlSelect _sqlSelect;
        private SelectType _selectType = SelectType.List;
        
        internal DataTaskBuilderRead(IDbConnection connection)
        {
            _connection = connection;
        }

        public IReadQueryModifiers<TTable,TArgs> Query(Func<QueryBuilderArgs<TTable,TArgs>, SqlExpression<TTable>> builder)
        {
            _queryBuilder = builder;
            return this;
        }
        
        public IReadQueryModifiers<TTable,TArgs> Query(string sql)
        {
            _sqlSelect = new QuerySqlSelect(sql, null);
            return this;
        } 
        
        public IReadQueryModifiers<TTable,TArgs> Query(string sql, object anonType)
        {
            _sqlSelect = new QuerySqlSelect(sql, anonType);
            return this;
        }

        public IQueryPipeBuilder AsList()
        {
            _selectType = SelectType.List;
            return this;
        }
        
        public IQueryPipeBuilder AsSingle()
        {
            _selectType = SelectType.Single;
            return this;
        }
        

        public IReadQueryModifiers<TTable, TArgs> OnEmptyOrNullRaiseError(string message = "no results found")
        {
            _onEmptyOrNullRaiseError = message;
            return this;
        }


        public PipeElement Build()
        {
            if (_queryBuilder is not null && _sqlSelect is not null)
            {
                throw new ArgumentException("You can't  configure both query with expression and sql");
            }
            return new DataQueryElement<TTable, TArgs>(_connection, _queryBuilder, _onEmptyOrNullRaiseError, _selectType, _sqlSelect);
        }
    }


    internal class DataQueryElement<TTable, TArgs> : PipeElement<TArgs>
    {
        private IDbConnection? _connection;
        private readonly Func<QueryBuilderArgs<TTable, TArgs>, SqlExpression<TTable>> _queryBuilder;
        private readonly string? _onEmptyOrNullRaiseError;
        private readonly SelectType _selectType;
        private readonly QuerySqlSelect? _querySqlSelect;

        internal DataQueryElement(IDbConnection? connection,
            Func<QueryBuilderArgs<TTable, TArgs>, SqlExpression<TTable>>? queryBuilder, string? onEmptyOrNullRaiseError,
            SelectType selectType, QuerySqlSelect? querySqlSelect)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
            _onEmptyOrNullRaiseError = onEmptyOrNullRaiseError;
            _selectType = selectType;
            _querySqlSelect = querySqlSelect;
        }

        private IDbConnection Db
        {
            get
            {
                return _connection ??= HostContext.AppHost.GetDbConnection();
            }
        }
        
        protected override async Task<PipeElementResult> OnExecuteAsync(TArgs args, PipelineState state)
        {
            var expression = _queryBuilder(new QueryBuilderArgs<TTable, TArgs>(Db.From<TTable>(), args));

            object result = null;
            if (_selectType == SelectType.List)
            {
                if (_queryBuilder is not null)
                {
                    result = await Db.SelectAsync(expression);
                }

                if (_querySqlSelect is {anonType: null} query)
                {
                    result = await Db.SelectAsync<TTable>(query.sql);
                }
                else if(_querySqlSelect is { } q)
                {
                    result = await Db.SelectAsync<TTable>(q.sql, q.anonType);
                }
            }else if (_selectType == SelectType.Single)
            {
                if (_queryBuilder is not null)
                {
                    result = await Db.SingleAsync(expression);
                }

                if (_querySqlSelect is {anonType: null} query)
                {
                    result = await Db.SingleAsync<TTable>(query.sql);
                }
                else if(_querySqlSelect is { } q)
                {
                    result = await Db.SingleAsync<TTable>(q.sql, q.anonType);
                }
            }

            if (!_onEmptyOrNullRaiseError.IsNullOrEmpty())
            {
                if ((result is IList {Count: 0}) || (result is TArgs[] {Length: 0}))
                {
                    return new PipeErrorResult {ErrorObject = _onEmptyOrNullRaiseError};   
                }

                if (result is null)
                {
                    return new PipeErrorResult {ErrorObject = _onEmptyOrNullRaiseError};
                }
            }
            
            return new PipeSuccessResult
            {
                Result = result
            };
        }
    }
}