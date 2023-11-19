using Interfaces;
using System;
using System.Collections.Generic;

namespace Pool
{ 
    public class ObjectPool<T> : IPool<T> where T : class
    {
        protected readonly Queue<T> storage;

        private readonly Func<T> createFunc;
        private readonly Action<T> getAction;
        private readonly Action<T> releaseAction;
        private readonly Action<T> destroyAction;

        public ObjectPool(
            Func<T> createFunc,
            Action<T> getAction = null,
            Action<T> releaseAction = null,
            Action<T> destroyAction = null,
            int defaultCapacity = 10)
        {
            this.getAction = getAction;
            this.releaseAction = releaseAction;
            this.destroyAction = destroyAction;
            this.createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));

            this.storage = new Queue<T>(defaultCapacity);

            Preload(defaultCapacity);
        }

        public T Get()
        {
            T item = storage.Count > 0 
                ? storage.Dequeue() 
                : createFunc.Invoke();         

            getAction?.Invoke(item);

            return item;
        }

        public void Release(T item)
        {
            releaseAction?.Invoke(item);
            storage.Enqueue(item);           
        }

        public void Clear()
        {
            if(destroyAction is not null)
            {
                foreach (var item in storage)
                {
                    destroyAction.Invoke(item);
                }
            }
            storage.Clear();
        }

        private void Preload(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                storage.Enqueue(createFunc.Invoke());
            }
        }
    }
}
