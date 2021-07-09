using NHibernate;
using NHibernate.Multi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Patterns.Repository
{
    // https://markheath.net/post/async-antipatterns
    // https://markheath.net/post/constraining-concurrent-threads-csharp
    public interface IRepository<T> where T : class
    {
        void AddEntity(T entity);
        Task<bool> AddEntityAsync(T entity, bool usingTransaction = true);
        Task<bool> AddEntityAsync(IEnumerable<T> collection, bool usingTransaction = true);
        void UpdateEntity(T entity);
        Task<bool> UpdateEntityAsync(T entity, bool usingTransaction = true);
        Task<bool> UpdateEntityAsync(IEnumerable<T> collection, bool usingTransaction = true);
        void DeleteEntity(T entity);
        Task<bool> DeleteEntityAsync(T entity, bool usingTransaction = true);
        Task<bool> DeleteEntityAsync(IEnumerable<T> collection, bool usingTransaction = true);
        void Delete(object key);
        Task<bool> DeleteAsync(object key, bool usingTransaction = true);
        Task<bool> DeleteWithListKeyAsync(IEnumerable<object> collection, bool usingTransaction = true);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllBySearchAsync(Expression<Func<T, bool>> search);
        IEnumerable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes);
        List<T> GetbySQLQuery(string Query, params SQLParam[] _params);
        ITransaction BeginTransaction();
        void CommitTransaction();
        Task<bool> CommitTransactionAsync();
        void RolbackTransaction();
        Task RolbackTransactionAsync();
        void SetFetchPage(int from, int maxResult);
        void ResetFetchPage();
        IList<T> GetAll(int pageIndex, int pageSize, out int total);
        IQueryable<T> GetPage<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType);
        (IList<T>, int) GetPageWithTotal<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType);
        Task<IList<T>> GetPageWithTransactionAsync<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType);
        Task<(IList<T>, int)> GetPageWithTransactionWithTotalAsync<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType);
        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        T GetByKey(object key);
        void BindSession(object obj);
        void UnbindSession(object obj);
        void Clear();
        void ClearSession();
        void SetBatchSize(int size);

        IQuery CreateDeleteQuery<TValue>(IEnumerable<string> collection, string columnName);
        Task<(IList<T>, int)> GetPageWithTotalAsync<TSelect>(IQueryable<T> query, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType);
        Task<(IList<T>, int)> GetPageWithTotalAsync<TSelect>(IQueryable<T> query, int pageIndex, int pageSize);
    }

    public class SQLParam
    {
        private string _ParameName;

        public string ParameName
        {
            get { return _ParameName; }
            set { _ParameName = value; }
        }
        private object _ParamValue;

        public object ParamValue
        {
            get { return _ParamValue; }
            set { _ParamValue = value; }
        }

        public SQLParam(string mParamName, object mParamValue)
        {
            _ParameName = mParamName;
            _ParamValue = mParamValue;
        }
    }
}
