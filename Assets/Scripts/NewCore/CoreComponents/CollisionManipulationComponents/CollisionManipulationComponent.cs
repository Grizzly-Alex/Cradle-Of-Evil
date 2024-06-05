using Interfaces;
using NewCoreSystem;
using UnityEngine;

namespace NewCore.CoreComponents.CollisionManipulationComponents
{
    public abstract class CollisionManipulationComponent : ILogicUpdate
    {
        protected readonly Core core;
        protected readonly CapsuleCollider2D entityCollider;

        public CollisionManipulationComponent(Core core)
        {
            this.core = core;
            entityCollider = core.GetComponentInParent<CapsuleCollider2D>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}
