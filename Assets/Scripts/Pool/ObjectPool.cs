using Interfaces;
using System;
using System.Collections.Generic;

namespace Pool
{ 
    public class ObjectPool<T> : IPool<T> where T : class
    {
        protected readonly Queue<T> _storage;

        private readonly Func<T> _createFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T> _releaseAction;
        private readonly Action<T> _destroyAction;

        public ObjectPool(
            Func<T> createFunc,
            Action<T> getAction = null,
            Action<T> releaseAction = null,
            Action<T> destroyAction = null,
            int defaultCapacity = 10)
        {
            _getAction = getAction;
            _releaseAction = releaseAction;
            _destroyAction = destroyAction;
            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));

            _storage = new Queue<T>(defaultCapacity);

            Preload(defaultCapacity);
        }

        public T Get()
        {
            T item = _storage.Count > 0 
                ? _storage.Dequeue() 
                : _createFunc.Invoke();         

            _getAction?.Invoke(item);

            return item;
        }

        public void Release(T item)
        {
            _releaseAction?.Invoke(item);
            _storage.Enqueue(item);           
        }

        public void Clear()
        {
            if(_destroyAction is not null)
            {
                foreach (var item in _storage)
                {
                    _destroyAction.Invoke(item);
                }
            }
            _storage.Clear();
        }

        private void Preload(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                _storage.Enqueue(_createFunc.Invoke());
            }
        }
    }
}
