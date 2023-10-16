using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerLevitationDashState : PlayerAbilityState
    {
        private readonly int hashLevitationDash = Animator.StringToHash("LevitationDash");

        public PlayerLevitationDashState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
