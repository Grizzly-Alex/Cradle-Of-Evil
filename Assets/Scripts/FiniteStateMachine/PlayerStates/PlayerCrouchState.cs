using Entities;
using Pool.ItemsPool;
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

            player.Input.SitStandEvent += OnStand;
            player.Input.JumpEvent += OnJump;

            player.Animator.Play(hashIdle);
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.SitStandEvent -= OnStand;
            player.Input.JumpEvent -= OnJump;
        }


        #region Input
        private void OnStand()
        {
            stateMachine.ChangeState(player.SitStandState);            
        }

		private void OnJump()
        {
            player.SetColliderHeight(player.Data.StandColiderHeight);

            stateMachine.ChangeState(player.JumpState);
        }
        #endregion
    }
}