using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace StackX.Flow.Data
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
        
        public IReadQueryModifiers<TTable, TArgs> Query(string sql, Dictionary<string, object> dic);
    }

    public interface IReadQueryModifiers<TTable, TArgs>
    {
        public IReadQueryModifiers<TTable, TArgs> OnEmptyOrNullRaiseError(string message = "no results found");
        
        public IQueryPipeBuilder List();

        public IQueryPipeBuilder Single();
    }
    
    public interface IQueryPipeBuilder
    {
        FlowElement Build();
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

        public IReadQueryModifiers<TTable, TArgs> Query(string sql, Dictionary<string, object> dic)
        {
            _sqlSelect = new QuerySqlSelect(sql, dic);
            return this;
        }

        public IQueryPipeBuilder List()
        {
            _selectType = SelectType.List;
            return this;
        }
        
        public IQueryPipeBuilder Single()
        {
            _selectType = SelectType.Single;
            return this;
        }
        

        public IReadQueryModifiers<TTable, TArgs> OnEmptyOrNullRaiseError(string message = "no results found")
        {
            _onEmptyOrNullRaiseError = message;
            return this;
        }


        public FlowElement Build()
        {
            if (_queryBuilder is not null && _sqlSelect is not null)
            {
                throw new ArgumentException("You can't  configure both query with expression and sql");
            }
            return new DataQueryElement<TTable, TArgs>(_connection, _queryBuilder, _onEmptyOrNullRaiseError, _selectType, _sqlSelect);
        }
    }


    internal class DataQueryElement<TTable, TArgs> : FlowElement<TArgs>
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
        
        protected override async Task<FlowElementResult> OnExecuteAsync(TArgs args, FlowState state)
        {
            var expression = _queryBuilder(new QueryBuilderArgs<TTable, TArgs>(Db.From<TTable>(), args));

            object result = (_queryBuilder, _querySqlSelect, _selectType) switch
            {
                //List
                (not null, null, SelectType.List) => await Db.SelectAsync(expression),
                (null, {anonType: null}, SelectType.List) => await Db.SelectAsync<TTable>(_querySqlSelect.sql),
                (null, {anonType: Dictionary<string, object> anonType}, SelectType.List) => await
                    Db.SelectAsync<TTable>(
                        _querySqlSelect.sql, anonType),
                (null, not null, SelectType.List) => await Db.SelectAsync<TTable>(_querySqlSelect.sql,
                    _querySqlSelect.anonType),
                //Single
                (not null, null, SelectType.Single) => await Db.SingleAsync(expression),
                (null, {anonType: null}, SelectType.Single) => await Db.SingleAsync<TTable>(_querySqlSelect.sql),
                (null, {anonType: Dictionary<string, object> anonType}, SelectType.Single) => await Db
                    .SingleAsync<TTable>(
                        _querySqlSelect.sql, anonType),
                (null, not null, SelectType.Single) => await Db.SingleAsync<TTable>(_querySqlSelect.sql,
                    _querySqlSelect.anonType)
            };

            return (result, _onEmptyOrNullRaiseError.IsNullOrEmpty()) switch
            {
                (result: IList {Count: 0}, false) => new FlowErrorResult {ErrorObject = _onEmptyOrNullRaiseError},
                (result: TArgs[] {Length: 0}, false) => new FlowErrorResult {ErrorObject = _onEmptyOrNullRaiseError},
                (null, true) => new FlowErrorResult {ErrorObject = _onEmptyOrNullRaiseError},
                _ => new FlowSuccessResult() {Result = result}
            };
        }
    }
}