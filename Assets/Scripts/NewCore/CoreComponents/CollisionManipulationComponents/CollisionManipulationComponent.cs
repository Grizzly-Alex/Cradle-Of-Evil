using Interfaces;
using NewCoreSystem;
using NewCoreSystem.CoreComponents;
using UnityEngine;

namespace NewCore.CoreComponents.CollisionManipulationComponents
{
    [RequireComponent(typeof(CollisionManipulation))]
    public abstract class CollisionManipulationComponent : MonoBehaviour, ILogicUpdate
    {
        protected Core core;
        protected CapsuleCollider2D entityCollider;


        protected virtual void Start()
        {
            this.core = GetComponent<CollisionManipulation>().Core;
            entityCollider = core.GetComponentInParent<CapsuleCollider2D>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}
