using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerOnWallState : PlayerState
    {
        public Vector2 DetectedPos { private get; set; }

        private readonly int landingOnWall = Animator.StringToHash("LandingOnWall");
        private bool isGrounded;
        private Vector2 holdPosition;

        public PlayerOnWallState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Core.VisualFx.CreateDust(DustType.Tiny, DetectedPos, player.transform.rotation);

            player.Input.JumpEvent += OnJump;

			player.DashState.ResetAmountOfDash();
			player.JumpState.ResetAmountOfJump();

            player.Core.Movement.SetVelocityZero();

             holdPosition.Set(
                DetectedPos.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * player.Core.Movement.FacingDirection,
                DetectedPos.y - player.BodyCollider.size.y + player.BodyCollider.bounds.max.y - player.Core.Sensor.WallSensor.position.y);

            player.transform.position = holdPosition;
            player.Core.Movement.FreezePosY();

            player.Animator.Play(landingOnWall);
        }

        public override void Update()
        {
            base.Update();

            if (isGrounded)
            {
                stateMachine.ChangeState(player.StandState);
            }          
        }

        public override void Exit()
        {
            base.Exit();

            player.Core.Movement.ResetFreezePos();
            player.Input.JumpEvent -= OnJump;
        }

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }

        private void OnJump()
        {
            if (!isAnimFinished) return;

            stateMachine.ChangeState(player.JumpState);

            player.Core.VisualFx.CreateDust(DustType.JumpFromWall,
                new Vector2()
                {
                    x = player.Core.Movement.FacingDirection != 1
                        ? player.BodyCollider.bounds.max.x
                        : player.BodyCollider.bounds.min.x,
                    y = player.BodyCollider.bounds.min.y,
                },
                player.transform.rotation);
        }
    }
}
