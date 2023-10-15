using UnityEngine;
using Interfaces;

namespace CoreSystem.Components
{
    public abstract class CoreComponent: MonoBehaviour, ILogicUpdate
    {
        protected Core core;

        protected virtual void Awake()
        {
            core = GetComponent<Core>();
        }

        protected virtual void Start()
        {
        }


        public virtual void LogicUpdate()
        {
        }
    }
}