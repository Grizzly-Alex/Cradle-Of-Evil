using NewCore.CoreComponents.PhysicsComponents;
using NewCoreSystem;
using NewCoreSystem.CoreComponents;
using UnityEngine;

namespace NewCore.CoreComponents.SensorDetectComponents
{
    [RequireComponent(typeof(SensorDetect))]
    public abstract class SensorDetectComponent : MonoBehaviour
    {
        protected Core core;
        protected CapsuleCollider2D entityCollider;

        protected Transform sensor;
        protected abstract Vector2 InitSensorPosition { get; }
        protected abstract string SensorName { get; }


        protected virtual void Awake()
        {
            this.core = GetComponent<SensorDetect>().Core;
            entityCollider = core.GetComponentInParent<CapsuleCollider2D>();
            CreateSensor();          
        }

        private void CreateSensor()
        {
            GameObject obj = new GameObject(SensorName);
            obj.transform.parent = transform;
            obj.transform.position = InitSensorPosition;
            sensor = obj.transform;
        }
    }
}