using HouseofCat.Library.Extensions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Multi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Core.Patterns.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected ISession session;
        protected ITransaction transaction;
        private int FromIndex { get; set; } = -1;
        private int MaxResult { get; set; } = 0;
        public IQueryable<T> Query { get => session.Query<T>(); }
        public int MaxThreads { get; private set; } = 100;

        public Repository(ISession session)
        {
            this.session = session;
        }

        public void AddEntity(T entity)
        {
            session.Save(entity);
        }

        public void UpdateEntity(T entity)
        {
            session.Update(entity);
        }

        public void DeleteEntity(T entity)
        {
            session.Delete(entity);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return session.Query<T>().ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await session.Query<T>().ToListAsync();
        }

        public IEnumerable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = session.Query<T>();
            foreach (var include in includes)
            {
                query = query.Fetch(include);
            }
            return query.AsEnumerable();
        }

        public void Delete(object key)
        {
            T entity = GetByKey(key);
            DeleteEntity(entity);
        }

        public List<T> GetbySQLQuery(string Query, params SQLParam[] _params)
        {
            IQuery q = session.CreateSQLQuery(Query).AddEntity(typeof(T));
            foreach (SQLParam _para in _params)
            {
                q.SetParameter(_para.ParameName, _para.ParamValue);
            }
            if (FromIndex >= 0 && MaxResult > 0)
            {
                q.SetFirstResult(FromIndex);
                q.SetMaxResults(MaxResult);
                List<T> ret = q.List<T>() as List<T>;
                ResetFetchPage();
                return ret;
            }
            return q.List<T>() as List<T>;
        }

        public ITransaction BeginTransaction()
        {
            transaction = session.BeginTransaction();
            return transaction;
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RolbackTransaction()
        {
            transaction.Rollback();
        }

        public void SetFetchPage(int from, int maxResult)
        {
            FromIndex = from;
            MaxResult = maxResult;
        }

        public void ResetFetchPage()
        {
            FromIndex = -1;
            MaxResult = 0;
        }

        public IList<T> GetAll(int pageIndex, int pageSize, out int total)
        {
            IQueryBatch queries = session.CreateQueryBatch()
                .Add("list", session.Query<T>().Skip(pageSize * pageIndex).Take(pageSize))
                .Add("count", session.Query<T>(), q => q.Count());
            total = queries.GetResult<int>("count").Single();
            return queries.GetResult<T>("list");
        }

        public IQueryable<T> GetPage<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Asc:
                    return Query.Where(search).OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
                case OrderType.Desc:
                    return Query.Where(search).OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
                default:
                    return Query.Where(search).Skip(pageSize * pageIndex).Take(pageSize);
            }
        }

        public (IList<T>, int) GetPageWithTotal<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType)
        {
            IQueryBatch queries;
            int total;
            switch (orderType)
            {
                case OrderType.Asc:
                    queries = session.CreateQueryBatch()
                        .Add("list", session.Query<T>().Where(search).OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize))
                        .Add("count", session.Query<T>().Where(search), q => q.Count());
                    total = queries.GetResult<int>("count").Single();
                    return (queries.GetResult<T>("list"), total);
                case OrderType.Desc:
                    queries = session.CreateQueryBatch()
                        .Add("list", session.Query<T>().Where(search).OrderByDescending(orderBy).Skip(pageSize * pageIndex).Take(pageSize))
                        .Add("count", session.Query<T>().Where(search), q => q.Count());
                    total = queries.GetResult<int>("count").Single();
                    return (queries.GetResult<T>("list"), total);
                default:
                    queries = session.CreateQueryBatch()
                        .Add("list", session.Query<T>().Where(search).Skip(pageSize * pageIndex).Take(pageSize))
                        .Add("count", session.Query<T>().Where(search), q => q.Count());
                    total = queries.GetResult<int>("count").Single();
                    return (queries.GetResult<T>("list"), total);
            }
        }

        public async Task<IList<T>> GetPageWithTransactionAsync<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType)
        {
            try
            {
                BeginTransaction();
                var result = await GetPage(search, pageIndex, pageSize, orderBy, orderType).ToListAsync();
                await CommitTransactionAsync();
                return result;
            }
            catch (Exception)
            {
                await RolbackTransactionAsync();
            }
            return new List<T>();
        }

        public async Task<(IList<T>, int)> GetPageWithTransactionWithTotalAsync<TSelect>(Expression<Func<T, bool>> search, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType)
        {
            try
            {
                BeginTransaction();
                var result = GetPageWithTotal(search, pageIndex, pageSize, orderBy, orderType);
                await CommitTransactionAsync();
                return result;
            }
            catch (Exception)
            {
                await RolbackTransactionAsync();
            }
            return (new List<T>(), 0);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Query.FirstOrDefault(predicate);
        }

        public T GetByKey(object key)
        {
            var entity = session.Get(typeof(T), key);
            return (entity == null) ? default(T) : (T)entity;
        }

        public void BindSession(object obj)
        {
            if (obj is ICollection<T>)
            {
                foreach (T entity in (ICollection<T>)obj) session.Persist(entity);
            }
            else
                session.Persist(obj);
        }

        public void UnbindSession(object obj)
        {
            if (obj is ICollection<T>)
            {
                foreach (T entity in (ICollection<T>)obj) session.Evict(entity);
            }
            else
                session.Evict(obj);
        }

        public void Clear()
        {
            session.Clear();
        }

        public void ClearSession()
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllBySearchAsync(Expression<Func<T, bool>> search)
        {
            return await Query.Where(search).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Query.FirstOrDefaultAsync(predicate);
        }

        public void SetBatchSize(int size)
        {
            session = session.SetBatchSize(size);
        }

        public async Task<bool> AddEntityAsync(T entity, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                await session.SaveAsync(entity);
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> AddEntityAsync(IEnumerable<T> collection, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                foreach (var entity in collection)
                {
                    session.Save(entity);
                }
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteEntityAsync(T entity, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                await session.DeleteAsync(entity);
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(object key, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                var entity = await session.LoadAsync<T>(key);
                await session.DeleteAsync(entity);
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> CommitTransactionAsync()
        {
            try
            {
                await transaction.CommitAsync();
                return transaction.WasCommitted;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }
            }
        }

        public async Task RolbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                transaction.Dispose();
            }
        }

        public async Task<bool> DeleteEntityAsync(IEnumerable<T> collection, bool usingTransaction)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                SetBatchSize(100);
                foreach (var entity in collection)
                {
                    session.Delete(entity);
                }
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteWithListKeyAsync(IEnumerable<object> collection, bool usingTransaction)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                var persistentClass = session.GetSessionImplementation().GetEntityPersister(null, Activator.CreateInstance<T>());
                var tableName = persistentClass.EntityName;
                var fieldName = persistentClass.IdentifierPropertyName;
                var query = session.CreateQuery($"DELETE FROM {tableName} WHERE {fieldName} IN (:ids)");
                query.SetParameterList("ids", collection);
                query.ExecuteUpdate();
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateEntityAsync(T entity, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                await session.UpdateAsync(entity);
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateEntityAsync(IEnumerable<T> collection, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) BeginTransaction();
                SetBatchSize(100);
                foreach (var entity in collection)
                {
                    session.Update(entity);
                }
                if (usingTransaction) return await CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RolbackTransactionAsync();
                throw;
            }
        }

        public IQuery CreateDeleteQuery<TValue>(IEnumerable<string> values, string columnName)
        {
            var persistentClass = session.GetSessionImplementation().GetEntityPersister(null, Activator.CreateInstance<TValue>());
            var tableName = persistentClass.EntityName;
            var query = session.CreateQuery($"DELETE FROM {tableName} WHERE {columnName} IN (:values)");
            query.SetParameterList("values", values);
            return query;

        }

        public async Task<(IList<T>, int)> GetPageWithTotalAsync<TSelect>(IQueryable<T> query, int pageIndex, int pageSize, Expression<Func<T, TSelect>> orderBy, OrderType orderType)
        {
            IQueryBatch queries;
            int total;
            switch (orderType)
            {
                case OrderType.Desc:
                    queries = session.CreateQueryBatch()
                        .Add("list", query.OrderByDescending(orderBy).Skip(pageSize * pageIndex).Take(pageSize))
                        .Add("count", query, q => q.Count());
                    total = (await queries.GetResultAsync<int>("count", CancellationToken.None)).First();
                    return (queries.GetResult<T>("list"), total);
                case OrderType.Asc:
                default:
                    queries = session.CreateQueryBatch()
                        .Add("list", query.OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize))
                        .Add("count", query, q => q.Count());
                    total = (await queries.GetResultAsync<int>("count", CancellationToken.None)).First();
                    return (queries.GetResult<T>("list"), total);

            }
        }

        public async Task<(IList<T>, int)> GetPageWithTotalAsync<TSelect>(IQueryable<T> query, int pageIndex, int pageSize)
        {
            IQueryBatch queries;
            int total;

            queries = session.CreateQueryBatch()
                .Add("list", query.Skip(pageSize * pageIndex).Take(pageSize))
                .Add("count", query, q => q.Count());
            total = (await queries.GetResultAsync<int>("count", CancellationToken.None)).First();
            return (queries.GetResult<T>("list"), total);

        }
    }

    public enum OrderType
    {
        Asc,
        Desc
    }
}
