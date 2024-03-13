using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerStandState : PlayerOnGroundState
    {
        protected override float MoveSpeed => player.Data.StandMoveSpeed;
        protected override float ColiderHeight => player.Data.StandColiderHeight;
        protected override int HashIdle => Animator.StringToHash("IdleStand");
        protected override int HashMove => Animator.StringToHash("RunStart");


        public PlayerStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			
		}


        public override void Enter()
        {
            base.Enter();

            isCrouching = false;
            
            player.Input.SitStandEvent += OnSit;
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

		protected override void OnJump()
		{
            stateMachine.ChangeState(player.JumpState);

            player.Core.VisualFx.CreateDust(
                DustType.JumpFromGround,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }

		protected override void OnSlide()
		{
			stateMachine.ChangeState(player.SlideState);

            player.Core.VisualFx.CreateDust(
                DustType.StartSlide,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }
		#endregion
	}
}
