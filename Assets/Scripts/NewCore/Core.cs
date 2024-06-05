using UnityEngine;
using Interfaces;
using NewCoreSystem.CoreComponents;


namespace NewCoreSystem
{  
    public sealed class Core : MonoBehaviour, ILogicUpdate
    {
        public PhysicsManipulation Physics { get; private set; }
        public SensorDetect Sensor { get; private set; }
        public VisualFx VisualFx { get; private set; }
        public CollisionManipulation Body { get; private set; }


        public void Awake()
        {
            Sensor = GetComponentInChildren<SensorDetect>();
            Physics = GetComponentInChildren<PhysicsManipulation>();
            VisualFx = GetComponentInChildren<VisualFx>();
            Body = GetComponentInChildren<CollisionManipulation>();
        }

        public void LogicUpdate()
        {
            Physics.LogicUpdate();
            VisualFx.LogicUpdate();
        }
    }
}