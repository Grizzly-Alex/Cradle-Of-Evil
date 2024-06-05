using UnityEngine;
using Interfaces;

namespace NewCoreSystem.CoreComponents
{
    public abstract class CoreComponent: MonoBehaviour, ILogicUpdate
    {
        protected Core core;

        protected virtual void Awake()
        {
            core = GetComponentInParent<Core>();
        }

        public virtual void LogicUpdate()
        {
        }
    }
}