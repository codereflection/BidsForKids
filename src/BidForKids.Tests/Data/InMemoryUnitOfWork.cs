using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using BidsForKids.Data.Repositories;

namespace BidsForKids.Tests.Data
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        readonly IDictionary<Type, IQueryable> _dataSources
            = new Dictionary<Type, IQueryable>();

        private readonly object _lock = new object();

        public IDataSource<T> GetDataSource<T>() where T : class, new()
        {
            lock (_lock)
            {
                if (!_dataSources.ContainsKey(typeof(T)))
                    _dataSources.Add(typeof(T), new InMemoryDataSource<T>());

                return _dataSources[typeof(T)] as InMemoryDataSource<T>;
            }
        }

        public void SubmitChanges()
        { }

        public void SubmitChanges(ConflictMode conflictMode)
        { }

        public void Dispose()
        { }
    }
}