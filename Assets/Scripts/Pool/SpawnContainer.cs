using Pool.ItemsPool;
using UnityEngine;


namespace Pool
{
    public sealed class SpawnContainer : MonoBehaviour
    {
        [SerializeField] private PoolItem itemPrefab;
        [SerializeField] private int defaultPoolCapacity;

        public GameObjectSpawner<PoolItem> Spawner { get; private set; }

        private void Awake()
        {
            Spawner = new GameObjectSpawner<PoolItem>(itemPrefab, this.transform, defaultPoolCapacity);           
        }
    }
}