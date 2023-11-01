using Interfaces;
using UnityEngine;

namespace Pool
{
    public class GameObjectPool<T> : IPool<T>
    where T : MonoBehaviour
    {
        private readonly T gameObj;
        private readonly ObjectPool<T> pool;
        private readonly Transform container;

        public GameObjectPool(T gameObj, Transform container, int defaultCapacity, bool preload)
        {
            this.gameObj = gameObj;
            this.container = container;
            pool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy, defaultCapacity, preload);
        }

        #region Public methods
        public T Get() => pool.Get();
        public void Release(T monoObj) => pool.Release(monoObj);
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

