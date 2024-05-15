using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerCrouchState : PlayerOnGroundState
    {
        protected override float ColiderHeight => player.Data.CrouchColiderHeight;
        private readonly int hashIdle = Animator.StringToHash("IdleCrouch");


        public PlayerCrouchState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			
		}

        public override void Enter()
        {
            base.Enter();

            player.Animator.Play(hashIdle);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.Input.InputVertical != Vector2.down.y)
            {
                stateMachine.ChangeState(player.SitStandState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}