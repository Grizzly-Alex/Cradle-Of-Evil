using UnityEngine;
using System.Collections.Generic;

namespace Pool
{
    public class PoolManger : MonoBehaviour
    {
        private Dictionary<int, GameObjectPool> _poolDictionary = new Dictionary<int, GameObjectPool>();
        private Dictionary<int, int> _keyValuePairs = new Dictionary<int, int>();

        static PoolManger _instance;
        public static PoolManger Instance => _instance ??= FindObjectOfType<PoolManger>();

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();
            Transform container = GetContainer(prefab.name);

            if (!_poolDictionary.ContainsKey(poolKey))
            {               
                _poolDictionary.Add(poolKey, new GameObjectPool(prefab, container, poolSize));
            }
        }

        public void GetFromPool(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (_poolDictionary.TryGetValue(poolKey, out GameObjectPool pool))
            {
                GameObject obj = pool.Get();
                obj.transform.SetPositionAndRotation(position, rotation);
                int keyValuePair = obj.GetInstanceID();

                if (!_keyValuePairs.ContainsKey(keyValuePair))
                {
                    _keyValuePairs.Add(keyValuePair, poolKey);
                }
            }
        }

        public void ReturnToPool(GameObject prefab)
        {
            if (_keyValuePairs.TryGetValue(prefab.GetInstanceID(), out int poolKey))
            {
                if (_poolDictionary.ContainsKey(poolKey))
                {
                    _poolDictionary[poolKey].Release(prefab);
                }
            }
        }

        private Transform GetContainer(string name)
        {
            GameObject poolHolder = new GameObject($"Container [{name}]");
            poolHolder.transform.parent = transform;
            return poolHolder.transform;
        }
    }
}