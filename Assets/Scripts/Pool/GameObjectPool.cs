using Interfaces;
using Pool.ItemsPool;
using UnityEngine;

namespace Pool
{
    public class GameObjectPool : IPool<GameObject>
    {
        private readonly bool _hasPoolObjectComponent;
        private readonly PoolObject _poolObjectScript;
        private readonly GameObject _gameObject;
        private readonly Transform _container;
        private readonly ObjectPool<GameObject> _pool;

        public GameObjectPool(GameObject gameObject, Transform container, int defaultPoolCapacity)
        {
            _gameObject = gameObject;
            _container = container;
            _poolObjectScript = gameObject.GetComponent<PoolObject>();
            _hasPoolObjectComponent = gameObject.TryGetComponent(out _poolObjectScript);
            _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, defaultPoolCapacity);
        }

        #region Public methods
        public GameObject Get() => _pool.Get();
        public void Release(GameObject gameObject) => _pool.Release(gameObject);
        public void Clear() => _pool.Clear();
        #endregion

        #region On methods
        private GameObject OnCreate()
        {
            if (_hasPoolObjectComponent)
            {
                return _poolObjectScript.Create(_container);
            }
            else
            {
                return GameObject.Instantiate(_gameObject, _container);
            }
        }

        private void OnDestroy(GameObject obj)
        {
            if (_hasPoolObjectComponent)
            {
                _poolObjectScript.Destroy(obj);
            }
            else
            {
                GameObject.Destroy(obj);
            }
        }

        private void OnGet(GameObject obj)
        {
            if (_hasPoolObjectComponent)
            {               
                _poolObjectScript.Get(obj);
            }
            else
            {
                obj.SetActive(true);
            }
        }

        private void OnRelease(GameObject obj)
        {
            if (_hasPoolObjectComponent)
            {
                _poolObjectScript.Release(obj);
            }
            else
            {
                obj.SetActive(false);
            }
        }
        #endregion
    }
}