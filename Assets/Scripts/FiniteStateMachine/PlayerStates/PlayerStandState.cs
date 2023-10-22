using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerStandState : PlayerOnGroundState
    {
        private readonly int hashIdle = Animator.StringToHash("IdleStand");
        private readonly int hashRun = Animator.StringToHash("RunStart");

        protected override float MoveSpeed => player.Data.StandMoveSpeed;

		public PlayerStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			
		}

        public override void Enter()
        {
            base.Enter();
            
            player.Input.SitStandEvent += OnSit;

            player.SetColliderHeight(player.Data.StandColiderHeight);

            if (player.Input.NormInputX != 0)
            {
                player.Animator.Play(hashRun);
            }
            else
            {
                player.Animator.Play(hashIdle);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.SitStandEvent -= OnSit;
        }

        #region Input
        private void OnSit()
        {
            stateMachine.ChangeState(player.SitStandState);
        }

		protected override void OnJump()
		{
            stateMachine.ChangeState(player.JumpState);
        }

		protected override void OnSlide()
		{
			stateMachine.ChangeState(player.SlideState);
		}
		#endregion
	}
}
