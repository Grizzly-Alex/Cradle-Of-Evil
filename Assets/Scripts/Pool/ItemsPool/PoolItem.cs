using Interfaces;
using System;
using UnityEngine;

namespace Pool.ItemsPool
{
    public class PoolItem : MonoBehaviour, IPoolable<PoolItem>
    {
        protected Action<PoolItem> returnToPool;

        public virtual void Initialize(Action<PoolItem> returnAction)
        {
            returnToPool = returnAction;
        }

        public virtual void GetFromPool()
        {
            gameObject.SetActive(true);
        }

        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

        protected void OnDisable()
        {
            returnToPool?.Invoke(this);
        }
    }
}
