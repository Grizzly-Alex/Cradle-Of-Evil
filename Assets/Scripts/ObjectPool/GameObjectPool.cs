using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class GameObjectPool<T> : IObjectPool<T>
    where T : MonoBehaviour
    {
        private readonly T gameObj;
        private readonly ObjectPool<T> pool;
        private readonly Transform container;

        public int CountActive => pool.CountActive;
        public int CountInactive => pool.CountInactive;

        public GameObjectPool(T gameObj, Transform container, int defaultCapacity, int maxSize)
        {
            this.gameObj = gameObj;
            this.container = container;
            pool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy, true, defaultCapacity, maxSize);

            for (int i = 0; i < defaultCapacity; i++)
            {
                pool.Release(ObjectCreate());
            }
        }

        #region Public methods
        public T Get() => pool.Get();
        public void Release(T monoObj) => pool.Release(monoObj);
        public PooledObject<T> Get(out T v) => pool.Get(out v);
        public void Clear() => pool.Clear();
        #endregion

        #region On methods
        private void OnDestroy(T obj) => GameObject.Destroy(obj);
        private void OnRelease(T obj) => obj.gameObject.SetActive(false);
        private void OnGet(T obj) => obj.gameObject.SetActive(true);
        private T OnCreate() => ObjectCreate();
        #endregion

        #region Private methods
        private T ObjectCreate()
        {
            var obj = GameObject.Instantiate(gameObj, container);
            obj.gameObject.SetActive(true);
            return obj;
        }
        #endregion
    }
}

