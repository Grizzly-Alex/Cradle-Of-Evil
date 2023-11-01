using System;
using UnityEngine;

namespace Pool.ItemsPool
{
    public abstract class ItemPool<T> : MonoBehaviour
        where T : Delegate
    {
        public T Release { get; set; }
    }
}
