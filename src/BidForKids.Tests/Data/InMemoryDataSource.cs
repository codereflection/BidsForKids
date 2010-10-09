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
        private readonly T _inner;
        private InMemoryTrackedState _state;

        public InMemoryInMemoryTrackedObject(T trackedObject)
        {
            _inner = trackedObject;
        }

        public T Inner
        {
            get { return _inner; }
        }

        public InMemoryTrackedState State
        {
            get { return _state; }
        }

        object IInMemoryTrackedObject.Inner
        {
            get { return _inner; }
        }

        public void ChangedState(InMemoryTrackedState state)
        {
            _state = state;
        }

        public void AcceptChanges()
        {
            _state = InMemoryTrackedState.Undefined;
        }
    }

    public class InMemoryDataSource<T> : DataSource<T>, IInMemoryTrackingContainer where T : class, new()
    {
        private readonly IDictionary<T, InMemoryInMemoryTrackedObject<T>> _trackedObjects
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
            if (!_trackedObjects.ContainsKey(entity))
                _trackedObjects.Add(entity, new InMemoryInMemoryTrackedObject<T>(entity));
            _source = _trackedObjects.Keys.AsQueryable();
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

        public override void Save(T entity)
        {
            Track(entity);
            _trackedObjects[entity].ChangedState(InMemoryTrackedState.Added);
        }

        public override void Delete(T entity)
        {
            Track(entity);
            _trackedObjects[entity].ChangedState(InMemoryTrackedState.Deleted);
        }

        IDictionary IInMemoryTrackingContainer.Data
        {
            get { return (IDictionary)_trackedObjects; }
        }
    }
}