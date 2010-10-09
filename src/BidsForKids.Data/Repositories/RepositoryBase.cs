﻿using System;
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
        protected readonly IDataSource<T> _source;
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _source = _unitOfWork.GetDataSource<T>();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        public Expression Expression
        {
            get { return _source.Expression; }
        }

        public Type ElementType
        {
            get { return _source.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return _source.Provider; }
        }

        public T GetById(int id)
        {
            return _source.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _source.GetAll();
        }

        public void Add(T entity)
        {
            _source.Add(entity);
        }

        public void Delete(T entity)
        {
            _source.Delete(entity);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IDataSource<T1> GetDataSource<T1>() where T1 : class, new()
        {
            return _source as IDataSource<T1>;
        }

        public void SubmitChanges()
        {
            _unitOfWork.SubmitChanges();
        }

        public void SubmitChanges(ConflictMode conflictMode)
        {
            _unitOfWork.SubmitChanges(conflictMode);
        }
    }
}