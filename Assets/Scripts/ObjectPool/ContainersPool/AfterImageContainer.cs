using ObjectPool.ItemsPool;
using UnityEngine;


namespace ObjectPool.ContainersPool
{
    public sealed class AfterImageContainer : MonoBehaviour
    {
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maxCapcity;
        [SerializeField] private AfterImageSprite itemPrefab;

        public GameObjectPool<AfterImageSprite> Pool { get; private set; }

        private void Awake()
        {
            Pool = new GameObjectPool<AfterImageSprite>(itemPrefab, transform, defaultCapacity, maxCapcity);
        }
    }
}