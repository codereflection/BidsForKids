using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace BidsForKids.Data.Repositories
{
    public interface IRepository<T> : IDataSource<T>, IUnitOfWork where T : class, new()
    {
    }

    public class RepositoryBase<T> : IRepository<T> where T : class, new()
    {
        protected readonly IDataSource<T> Source;
        private readonly IUnitOfWork unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Source = this.unitOfWork.GetDataSource<T>();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        public Expression Expression
        {
            get { return Source.Expression; }
        }

        public Type ElementType
        {
            get { return Source.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return Source.Provider; }
        }

        public T GetById(int id)
        {
            return Source.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Source.GetAll();
        }

        public void Add(T entity)
        {
            Source.Add(entity);
        }

        public void Delete(T entity)
        {
            Source.Delete(entity);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public IDataSource<T1> GetDataSource<T1>() where T1 : class, new()
        {
            return Source as IDataSource<T1>;
        }

        public void SubmitChanges()
        {
            unitOfWork.SubmitChanges();
        }

        public void SubmitChanges(ConflictMode conflictMode)
        {
            unitOfWork.SubmitChanges(conflictMode);
        }
    }
}