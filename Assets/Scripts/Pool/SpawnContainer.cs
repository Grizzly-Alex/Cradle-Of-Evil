using Pool.ItemsPool;
using UnityEngine;


namespace Pool
{
    public sealed class SpawnContainer : MonoBehaviour
    {
        [SerializeField] private PoolItem itemPrefab;
        [SerializeField] private int defaultPoolCapacity;

        //public GameObjectPool<PoolItem> Spawner { get; private set; }

        //private void Awake()
        //{
        //    Spawner = new GameObjectPool<PoolItem>(itemPrefab, this.transform, defaultPoolCapacity);           
        //}
    }
}