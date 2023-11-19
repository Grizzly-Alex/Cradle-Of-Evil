using UnityEngine;

namespace Pool.ItemsPool
{
    public class PoolObject : MonoBehaviour
    {
        public virtual void Get(GameObject obj)
        {
            obj.SetActive(true);
        }

        public virtual void Release(GameObject obj)
        {
            obj.SetActive(false);
        }

        public virtual GameObject Create(Transform container)
        {
            return GameObject.Instantiate(gameObject, container);
        }

        public virtual void Destroy(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
    }
}
