using Interfaces;
using NewCoreSystem;
using UnityEngine;

namespace NewCore.CoreComponents.VisualFxComponents
{
    public abstract class VisualFxComponent : ILogicUpdate
    {
        protected readonly Core core;
        protected Transform entityTransform;


        public VisualFxComponent(Core core)
        {
            this.core = core;
            entityTransform = core.GetComponentInParent<Transform>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}
