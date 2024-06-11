using Interfaces;
using NewCoreSystem;
using NewCoreSystem.CoreComponents;
using UnityEngine;

namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    [RequireComponent(typeof(PhysicsManipulation))]
    public abstract class PhysicsManipulationComponent : MonoBehaviour, ILogicUpdate
    {
        protected Core core;
        protected Rigidbody2D body;

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            this.core = GetComponent<PhysicsManipulation>().Core;
            body = core.GetComponentInParent<Rigidbody2D>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}
