using UnityEngine;
using System.Collections.Generic;
using Pool.ItemsPool;

namespace Pool
{
    public class PoolManger : MonoBehaviour
    {
        Dictionary<int, Queue<ObjectInstance>> _poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

        static PoolManger _instance;
        public static PoolManger Instance => _instance ??= FindObjectOfType<PoolManger>();

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            if (!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

                for (int i = 0; i < poolSize; i++)
                {
                    ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                    _poolDictionary[poolKey].Enqueue(newObject);
                    newObject.SetParent(poolHolder.transform);
                }
            }
        }

        public void ReuseObject(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();
            if (_poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance objectToReuse = _poolDictionary[poolKey].Dequeue();
                _poolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.Reuse(position, rotation);
            }
        }

        public class ObjectInstance
        {
            private GameObject gameObject;
            private Transform transform;

            private bool hasPoolObjectComponent;
            private PoolObject poolObjectScript;

            public ObjectInstance(GameObject objectInstance)
            {
                gameObject = objectInstance;
                transform = gameObject.transform;
                gameObject.SetActive(false);

                if (gameObject.GetComponent<PoolObject>())
                {
                    hasPoolObjectComponent = true;
                    poolObjectScript = gameObject.GetComponent<PoolObject>();
                }
            }

            public void Reuse(Vector2 position, Quaternion rotation)
            {
                if(hasPoolObjectComponent)
                {
                    poolObjectScript.OnObjectReuse();
                }

                gameObject.SetActive(true);
                gameObject.transform.SetPositionAndRotation(position, rotation);
            }

            public void SetParent(Transform parent)
            {
                transform.parent = parent;
            }
        }
    }
}
