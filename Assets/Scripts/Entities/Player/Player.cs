using FiniteStateMachine;
using FiniteStateMachine.PlayerStates;
using UnityEngine;

namespace Entities
{
    public sealed class Player : Entity
    {
        [field: SerializeField]
        public PlayerData Data { get; private set; }
        public InputReader Input { get; private set; }
        public PlayerStatesContainer States { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            States = new PlayerStatesContainer(stateMachine, this);
        }

        protected override void Start()
        {
            base.Start();
            Input = GetComponent<InputReader>();
            stateMachine.InitState(States.Stand);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}