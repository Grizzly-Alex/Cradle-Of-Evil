using Interfaces;
using UnityEngine;


namespace CoreSystem.CoreComponents.PhysicsManipulationComponents
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
