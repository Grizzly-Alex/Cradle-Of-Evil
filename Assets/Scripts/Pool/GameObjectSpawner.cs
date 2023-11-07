using Interfaces;
using UnityEngine;

namespace Pool
{
    public class GameObjectSpawner<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly T gameObj;
        private readonly ObjectPool<T> pool;
        private readonly Transform container;

        public GameObjectSpawner(T gameObj, Transform container, int defaultPoolCapacity)
        {
            this.gameObj = gameObj;
            this.container = container;
            pool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy, defaultPoolCapacity);
        }

        #region Public methods
        public T Spawn() => pool.Get();
        public void Backstage(T monoObj) => pool.Release(monoObj);
        public void DestroyAll() => pool.Clear();
        #endregion

        #region On methods
        private void OnDestroy(T obj) => GameObject.Destroy(obj);
        private void OnRelease(T obj) => obj.ReturnToPool();
        private void OnGet(T obj)
        {
            obj.GetFromPool();
            obj.Initialize(pool.Release);
        }
        private T OnCreate() => GameObject.Instantiate(gameObj, container);

        #endregion
    }
}