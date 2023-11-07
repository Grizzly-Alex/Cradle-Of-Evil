using System;

namespace Interfaces
{
    public interface IPoolable<T>
    {
        public void Initialize(Action<T> returnAction);
        public void GetFromPool();
        public void ReturnToPool();
    }
}