using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerCrouchState : PlayerOnGroundState
    {
        private readonly int hashIdle = Animator.StringToHash("IdleCrouch");
        private bool isTouchedRoof;

		protected override float MoveSpeed => player.Data.CrouchMoveSpeed;

		public PlayerCrouchState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			
		}

        public override void Enter()
        {
            base.Enter();

            player.Input.SitStandEvent += OnStand;

            player.SetColliderHeight(player.Data.CrouchColiderHeight);
            player.Animator.Play(hashIdle);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.SitStandEvent -= OnStand;
        }

        public override void DoCheck()
        {
            base.DoCheck();
            isTouchedRoof = player.Core.Sensor.IsCellingDetect();
        }

        #region Input
        private void OnStand()
        {
            if (!isTouchedRoof) stateMachine.ChangeState(player.SitStandState);            
        }

		protected override void OnJump()
        {
            if (!isTouchedRoof) stateMachine.ChangeState(player.JumpState);
        }

		protected override void OnSlide()
        {
            stateMachine.ChangeState(player.SlideState);
        }
        #endregion
    }
}