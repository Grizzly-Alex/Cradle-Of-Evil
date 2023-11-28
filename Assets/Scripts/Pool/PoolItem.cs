using System;
using UnityEngine;

namespace Pool
{
    [Serializable]
    public sealed class PoolItem
    {
        [field: SerializeField]
        public GameObject Prefab {  get; private set; }
        [field: SerializeField]
        public int PoolCount { get; private set; }
    }
}
