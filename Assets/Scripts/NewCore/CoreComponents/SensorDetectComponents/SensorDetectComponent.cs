using Interfaces;
using NewCoreSystem;
using UnityEngine;

namespace NewCore.CoreComponents.SensorDetectComponents
{
    public abstract class SensorDetectComponent : ILogicUpdate
    {
        protected readonly Core core;
        protected readonly CapsuleCollider2D entityCollider;

        public SensorDetectComponent(Core core)
        {
            this.core = core;
            entityCollider = core.GetComponentInParent<CapsuleCollider2D>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}
