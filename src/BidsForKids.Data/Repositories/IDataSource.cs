using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace BidsForKids.Data.Repositories
{
    public class DatabaseDataSource<T> : DataSource<T> where T : class, new()
    {
        private readonly DataContext _dataContext;

        public DatabaseDataSource(DataContext dataContext)
        {
            _dataContext = dataContext;

            if (dataContext == null)
                throw new ArgumentNullException("dataContext cannot be null");

            _source = _dataContext.GetTable<T>();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        public override Expression Expression
        {
            get { return _source.Expression; }
        }

        public override Type ElementType
        {
            get { return _source.ElementType; }
        }

        public override IQueryProvider Provider
        {
            get { return _source.Provider; }
        }

        public override void Add(T entity)
        {
            (_source as Table<T>).InsertOnSubmit(entity);
        }

        public override void Delete(T entity)
        {
            (_source as Table<T>).DeleteOnSubmit(entity);
        }
    }

    public abstract class DataSource<T> : IDataSource<T> where T : class, new()
    {
        public abstract IEnumerator<T> GetEnumerator();
        protected IQueryable<T> _source;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract Expression Expression { get; }

        public abstract Type ElementType { get; }

        public abstract IQueryProvider Provider { get; }

        public virtual T GetById(int id)
        {
            var itemParameter = Expression.Parameter(typeof (T), "item");

            var whereExpression =
                Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Property(
                            itemParameter,
                            typeof(T).GetPrimaryKey().Name),
                        Expression.Constant(id)),
                    new[] { itemParameter });

            return _source.Where(whereExpression).Single();
        }

        public abstract void Add(T entity);
        public abstract void Delete(T entity);

        public IEnumerable<T> GetAll()
        {
            return _source;
        }
    }

    public interface IDataSource<T> : IQueryable<T> where T : class, new()
    {
        T GetById(int id);
        void Add(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
    }
}