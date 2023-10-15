using CoreSystem.Components;
using Interfaces;
using UnityEngine;

namespace CoreSystem
{
    public sealed class Core : MonoBehaviour, ILogicUpdate
    {
        public Movement Movement { get; private set; }
        public Sensor Sensor { get; private set; }

        public void Awake()
        {
            Sensor = GetComponent<Sensor>();
            Movement = GetComponent<Movement>();
        }

        public void LogicUpdate()
        {
            Movement.LogicUpdate();
        }
    }
}