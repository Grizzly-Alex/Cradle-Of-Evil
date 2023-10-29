using System;
using UnityEngine;

namespace ObjectPool.ItemsPool
{
    public abstract class ItemPool<T> : MonoBehaviour
        where T : Delegate
    {
        public T Relise { get; set; }
    }
}
