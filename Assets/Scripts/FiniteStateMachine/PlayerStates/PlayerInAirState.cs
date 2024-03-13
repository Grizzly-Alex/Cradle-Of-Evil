using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerInAirState : PlayerState
    {
        private readonly int hashVelocityY = Animator.StringToHash("velocityY");
        private readonly int hashInAir = Animator.StringToHash("InAirState");

        private bool isGrounded;
        private bool isLedgeDetected;
        private bool isGrabWallDetected;
        private float сuгrentVelocityY;
        private float fallingForce;

        public bool UseDoubleJump {  get; set; }


        public PlayerInAirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

			player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnDash;
                       
            PlayInAirAnimation();
        }

        public override void Update()
        {
            base.Update();

            player.Animator.SetFloat(hashVelocityY, сuгrentVelocityY);
            player.Core.Movement.FlipToMovement(player.Input.NormInputX);
            player.Core.Movement.SetVelocityX(player.Input.NormInputX * player.Data.InAirMoveSpeed);

            if (isGrounded)
            {
                player.LandingState.LandingForce = fallingForce;
                stateMachine.ChangeState(player.LandingState);
            }
            else if (isLedgeDetected && сuгrentVelocityY <= 0.0f) 
            {
				stateMachine.ChangeState(player.LedgeClimbState);
            }
            else if (isGrabWallDetected && сuгrentVelocityY <= 0.0f)
            {
				stateMachine.ChangeState(player.OnWallState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnDash;

            ResetFallingForce();
        }

        public override void DoCheck()
        {
            TrackingFallingForce();

            isGrounded = player.Core.Sensor.IsGroundDetect();
            сuгrentVelocityY = player.Core.Movement.CurrentVelocity.y;

            if (isLedgeDetected = player.Core.Sensor.IsHorizonalLedgCornerDetect(out Vector2 ledgeCorner))
                player.LedgeClimbState.CornerPosition = ledgeCorner;

            if (isGrabWallDetected = player.Core.Sensor.IsGrabWallDetect())
                player.OnWallState.DetectedPos = player.Core.Sensor.WallHit.point;
        }

		public override void AnimationTrigger() 
            => player.Animator.Play(hashInAir);

		private void PlayInAirAnimation()
		{
			if (!UseDoubleJump)
			{
				player.Animator.Play(hashInAir);
			}
			else
			{
				player.Animator.Play(player.JumpState.GetDoubleJumpHashAnim());
				UseDoubleJump = false;
			}
		}

        private void TrackingFallingForce() 
        {
            if (сuгrentVelocityY < fallingForce)
            {
                fallingForce = сuгrentVelocityY;
            }
        }

        private void ResetFallingForce() => fallingForce = default;

        private void OnJump()
        {
            if (!player.JumpState.CanJump()) return;

            stateMachine.ChangeState(player.JumpState);
        }

        private void OnDash()
        {
			if (!player.DashState.CanDash()) return;

            stateMachine.ChangeState(player.DashState);
            player.Core.VisualFx.CreateDust(DustType.Dash, player.BodyCollider.bounds.center, player.transform.rotation);
        }
    }
}