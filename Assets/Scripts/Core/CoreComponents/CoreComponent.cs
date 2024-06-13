using UnityEngine;
using Interfaces;


namespace CoreSystem.CoreComponents
{
    public abstract class CoreComponent: MonoBehaviour, ILogicUpdate
    {
        public Core Core { get; private set; }

        protected virtual void Awake()
        {
            Core = GetComponentInParent<Core>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}