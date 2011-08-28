using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BidsForKids.Data.Repositories;

namespace BidsForKids.Tests.Data
{
    public interface IInMemoryTrackingContainer
    {
        IDictionary Data { get; }
    }

    public enum InMemoryTrackedState
    {
        Undefined,
        Added,
        Modified,
        Deleted
    }

    public interface IInMemoryTrackedObject
    {
        object Inner { get; }
    }

    public class InMemoryInMemoryTrackedObject<T> : IInMemoryTrackedObject where T : class, new()
    {
        private readonly T inner;
        private InMemoryTrackedState state;

        public InMemoryInMemoryTrackedObject(T trackedObject)
        {
            inner = trackedObject;
        }

        public T Inner
        {
            get { return inner; }
        }

        public InMemoryTrackedState State
        {
            get { return state; }
        }

        object IInMemoryTrackedObject.Inner
        {
            get { return inner; }
        }

        public void ChangedState(InMemoryTrackedState state)
        {
            this.state = state;
        }

        public void AcceptChanges()
        {
            state = InMemoryTrackedState.Undefined;
        }
    }

    public class InMemoryDataSource<T> : DataSource<T>, IInMemoryTrackingContainer where T : class, new()
    {
        private readonly IDictionary<T, InMemoryInMemoryTrackedObject<T>> trackedObjects
            = new Dictionary<T, InMemoryInMemoryTrackedObject<T>>();

        public InMemoryDataSource()
            : this(new List<T>())
        { }

        public InMemoryDataSource(List<T> source)
        {
            source.ForEach(Track);
        }

        private void Track(T entity)
        {
            if (!trackedObjects.ContainsKey(entity))
                trackedObjects.Add(entity, new InMemoryInMemoryTrackedObject<T>(entity));
            Source = trackedObjects.Keys.AsQueryable();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        public override Expression Expression
        {
            get { return Source.Expression; }
        }

        public override Type ElementType
        {
            get { return Source.ElementType; }
        }

        public override IQueryProvider Provider
        {
            get { return Source.Provider; }
        }

        public override void Add(T entity)
        {
            Track(entity);
            trackedObjects[entity].ChangedState(InMemoryTrackedState.Added);
        }

        public override void Delete(T entity)
        {
            Track(entity);
            trackedObjects[entity].ChangedState(InMemoryTrackedState.Deleted);
        }

        IDictionary IInMemoryTrackingContainer.Data
        {
            get { return (IDictionary)trackedObjects; }
        }
    }
}