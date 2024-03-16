using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerStandState : PlayerOnGroundState
    {
        protected override float ColiderHeight => player.Data.StandColiderHeight;

        private readonly int hashIsMoving = Animator.StringToHash("isMoving");
        private readonly int hashIdle = Animator.StringToHash("IdleStand");
        private readonly int hashMove = Animator.StringToHash("RunStart");

        public PlayerStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {			
		}

        public override void Enter()
        {
            base.Enter();

            player.Input.SitStandEvent += OnSit;
            player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnSlide;

            player.Animator.Play(player.Input.NormInputX != 0 ? hashMove : hashIdle);
        }

        public override void Update()
        {
            base.Update();

            player.Animator.SetBool(hashIsMoving, player.Input.NormInputX != 0);
            player.Core.Movement.Move(player.Data.StandMoveSpeed, player.Input.NormInputX);
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.SitStandEvent -= OnSit;
            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnSlide;
        }

        public override void AnimationTrigger()
        {
            if (player.Input.NormInputX != 0)
            {
                player.Core.VisualFx.CreateDust(
                    DustType.Tiny,
                    player.Core.Sensor.GroundHit.point,
                    player.transform.rotation);
            }
            else
            {
                player.Core.VisualFx.CreateDust(
                    DustType.AfterMove,
                    new Vector2()
                    {
                        x = player.Core.Movement.FacingDirection != 1
                            ? player.BodyCollider.bounds.min.x
                            : player.BodyCollider.bounds.max.x,
                        y = player.Core.Sensor.GroundHit.point.y,
                    },
                    player.transform.rotation);
            }
        }

        #region Input
        private void OnSit()
        {
            stateMachine.ChangeState(player.SitStandState);
        }

		private void OnJump()
		{
            stateMachine.ChangeState(player.JumpState); 
        }

		private void OnSlide()
		{
			stateMachine.ChangeState(player.SlideState);
        }
		#endregion
	}
}
