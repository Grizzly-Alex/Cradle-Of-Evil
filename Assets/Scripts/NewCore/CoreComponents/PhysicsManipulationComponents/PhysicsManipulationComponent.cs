using Interfaces;
using NewCoreSystem;
using UnityEngine;

namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    public abstract class PhysicsManipulationComponent : ILogicUpdate
    {
        protected readonly Core core;
        protected readonly Rigidbody2D body;

        public PhysicsManipulationComponent(Core core)
        {
            this.core = core;
            body = core.GetComponentInParent<Rigidbody2D>();
        }


        public virtual void LogicUpdate()
        {
        }
    }
}
