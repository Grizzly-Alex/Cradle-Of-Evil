using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerLedgeState : PlayerState
    {
        public static Vector2 CornerPosition { get; set; }
        protected abstract int AnimationHash { get; }

        public PlayerLedgeState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }


        public override void Enter()
        {
            base.Enter();
            player.Animator.Play(AnimationHash);
            player.Core.Movement.FreezePosY();
        }

        public override void Exit()
        {
            base.Exit();

            player.Core.Movement.ResetFreezePos();
        }
    }
}
