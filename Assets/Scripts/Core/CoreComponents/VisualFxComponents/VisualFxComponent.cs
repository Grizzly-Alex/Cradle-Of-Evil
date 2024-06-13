using Interfaces;
using UnityEngine;


namespace CoreSystem.CoreComponents.VisualFxComponents
{
    [RequireComponent(typeof(VisualFx))]
    public abstract class VisualFxComponent : MonoBehaviour, ILogicUpdate
    {
        protected Core core;
        protected Transform entityTransform;


        protected virtual void Start()
        {
            this.core = GetComponent<VisualFx>().Core;
            entityTransform = core.GetComponentInParent<Transform>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}