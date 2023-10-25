using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class AfterImagePoolTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject afterImagePrefab;

        private Queue<GameObject> availableObjects = new Queue<GameObject>();

        public static AfterImagePoolTest Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GrowPool();
        }

        private void GrowPool()
        {
            for (int i = 0; i < 10; i++)
            {
                var instanceToAdd = Instantiate(afterImagePrefab);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd);
            }
        }

        public void AddToPool(GameObject instance)
        {
            instance.SetActive(false);
            availableObjects.Enqueue(instance);
        }

        public GameObject GetFromPool()
        {
            if (!availableObjects.Any())
            {
                GrowPool();
            }

            var instance = availableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }
    }
}
