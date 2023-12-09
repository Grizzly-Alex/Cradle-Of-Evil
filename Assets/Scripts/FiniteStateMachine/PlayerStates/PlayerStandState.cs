using Entities;
using Pool.ItemsPool;
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
            stateMachine.ChangeState(player.States.SitStand);
        }

		protected override void OnJump()
		{
            stateMachine.ChangeState(player.States.Jump);

            player.Core.VisualFx.CreateDust(
                DustType.JumpFromGround,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }

		protected override void OnSlide()
		{
			stateMachine.ChangeState(player.States.Slide);

            player.Core.VisualFx.CreateDust(
                DustType.StartSlide,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }
		#endregion
	}
}
