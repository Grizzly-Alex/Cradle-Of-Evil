using Pool.ItemsPool;
using UnityEngine;

namespace Pool
{
    public class ObjectInstance
    {
        private GameObject gameObject;
        private Transform transform;

        private bool hasPoolObjectComponent;
        private PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }


        //public void OnGet(ObjectInstance obj)
        //{
        //    if (hasPoolObjectComponent)
        //    {
        //        obj.poolObjectScript.OnGet();
        //    }
        //    else
        //    {
        //        obj.gameObject.SetActive(true);
        //    }
        //}

        //public void OnRelease(ObjectInstance obj)
        //{
        //    if (hasPoolObjectComponent)
        //    {
        //        obj.poolObjectScript.OnRelease();
        //    }
        //    else
        //    {
        //        obj.gameObject.SetActive(false);
        //    }
        //}


        //public ObjectInstance OnCreate()
        //{

        //    if (hasPoolObjectComponent)
        //    {
        //        return new ObjectInstance(poolObjectScript.OnCreate());
        //    }
        //    else
        //    {
        //        return new ObjectInstance(Instantiate(gameObject));
        //    }
        //}

        //public void OnDestroy(ObjectInstance obj)
        //{
        //    if (hasPoolObjectComponent)
        //    {
        //        poolObjectScript.OnDestroy();
        //    }
        //    else
        //    {
        //        GameObject.Destroy(obj.gameObject);
        //    }
        //}


        //public void Reuse(Vector2 position, Quaternion rotation)
        //{
        //    //if(hasPoolObjectComponent)
        //    //{
        //    //poolObjectScript.OnObjectReuse();
        //    //}

        //    //gameObject.SetActive(true);
        //    gameObject.transform.SetPositionAndRotation(position, rotation);
        //}

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}
