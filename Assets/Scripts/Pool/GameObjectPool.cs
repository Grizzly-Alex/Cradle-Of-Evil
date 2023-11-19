using Interfaces;
using Pool.ItemsPool;
using UnityEngine;

namespace Pool
{
    public class GameObjectPool : IPool<GameObject>
    {
        private readonly bool hasPoolObjectComponent;
        private readonly PoolObject poolObjectScript;
        private readonly GameObject gameObject;
        private readonly Transform container;
        private readonly ObjectPool<GameObject> pool;

        public GameObjectPool(GameObject gameObject, Transform container, int defaultPoolCapacity)
        {
            this.gameObject = gameObject;
            this.container = container;
            this.poolObjectScript = gameObject.GetComponent<PoolObject>();
            this.hasPoolObjectComponent = gameObject.TryGetComponent(out poolObjectScript);
            this.pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, defaultPoolCapacity);
        }

        #region Public methods
        public GameObject Get() => pool.Get();
        public void Release(GameObject gameObject) => pool.Release(gameObject);
        public void Clear() => pool.Clear();
        #endregion

        #region On methods
        private GameObject OnCreate()
        {
            if (hasPoolObjectComponent)
            {
                return poolObjectScript.Create(container);
            }
            else
            {
                return GameObject.Instantiate(gameObject, container);
            }
        }

        private void OnDestroy(GameObject obj)
        {
            if (hasPoolObjectComponent)
            {
                poolObjectScript.Destroy(obj);
            }
            else
            {
                GameObject.Destroy(obj);
            }
        }

        private void OnGet(GameObject obj)
        {
            if (hasPoolObjectComponent)
            {               
                poolObjectScript.Get(obj);
            }
            else
            {
                obj.SetActive(true);
            }
        }

        private void OnRelease(GameObject obj)
        {
            if (hasPoolObjectComponent)
            {
                poolObjectScript.Release(obj);
            }
            else
            {
                obj.SetActive(false);
            }
        }
        #endregion
    }
}