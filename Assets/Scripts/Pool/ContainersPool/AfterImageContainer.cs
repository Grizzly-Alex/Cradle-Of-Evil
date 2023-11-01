using Pool.ItemsPool;
using UnityEngine;


namespace Pool.ContainersPool
{
    public sealed class AfterImageContainer : MonoBehaviour
    {
        [SerializeField] private int defaultCapacity;
        [SerializeField] private bool preload;
        [SerializeField] private AfterImageSprite itemPrefab;

        public GameObjectPool<AfterImageSprite> Pool { get; private set; }

        private void Awake()
        {
            Pool = new GameObjectPool<AfterImageSprite>(itemPrefab, transform, defaultCapacity, preload);
        }
    }
}