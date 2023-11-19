using UnityEngine;
using System.Collections.Generic;

namespace Pool
{
    public class PoolManger : MonoBehaviour
    {
        private Dictionary<int, GameObjectPool> poolDictionary = new Dictionary<int, GameObjectPool>();
        private Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

        static PoolManger _instance;
        public static PoolManger Instance => _instance ??= FindObjectOfType<PoolManger>();

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();
            Transform container = GetContainer(prefab.name);

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new GameObjectPool(prefab, container, poolSize));
            }
        }

        public void GetFromPool(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.TryGetValue(poolKey, out GameObjectPool pool))
            {
                GameObject obj = pool.Get();
                obj.transform.SetPositionAndRotation(position, rotation);
                int keyValuePair = obj.GetInstanceID();

                if (!keyValuePairs.ContainsKey(keyValuePair))
                {
                    keyValuePairs.Add(keyValuePair, poolKey);
                }
            }
        }

        public void ReturnToPool(GameObject prefab)
        {
            if (keyValuePairs.TryGetValue(prefab.GetInstanceID(), out int poolKey))
            {
                if (poolDictionary.ContainsKey(poolKey))
                {
                    poolDictionary[poolKey].Release(prefab);
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