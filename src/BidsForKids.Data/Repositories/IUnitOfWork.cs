using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace BidsForKids.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDataSource<T> GetDataSource<T>() where T : class, new();
        void SubmitChanges();
        void SubmitChanges(ConflictMode conflictMode);
    }

    public class DatabaseUnitOfWork : IUnitOfWork
    {
        private readonly DataContext dataContext;

        public DatabaseUnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IDataSource<T> GetDataSource<T>() where T : class, new()
        {
            return new DatabaseDataSource<T>(dataContext);
        }

        public void SubmitChanges()
        {
            var updates = dataContext.GetChangeSet().Updates;

            dataContext.SubmitChanges();
        }

        public void SubmitChanges(ConflictMode conflictMode)
        {
            dataContext.SubmitChanges(conflictMode);
        }

        public void Dispose()
        {
            dataContext.Dispose();
        }

    }
}