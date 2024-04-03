using Entities;
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
            player.Input.DashEvent += OnAirDash;

            player.Animator.Play(hashInAir);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Animator.SetFloat(hashVelocityY, сuгrentVelocityY);
            player.Core.Movement.FlipToDirection(player.Input.NormInputX);

            if (isGrounded)
            {
                player.LandingState.LandingForce = fallingForce;
                stateMachine.ChangeState(player.LandingState);
            }
            else if (isLedgeDetected && сuгrentVelocityY <= 0.0f) 
            {
				stateMachine.ChangeState(player.HangOnLedgeState);
            }
            else if (isGrabWallDetected && сuгrentVelocityY <= 0.0f)
            {
				stateMachine.ChangeState(player.OnWallState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            player.Core.Movement.SetVelocityX(player.Input.NormInputX * player.Data.InAirMoveSpeed);
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnAirDash;

            player.JumpState.DisableDoubleJumpFX();
            ResetFallingForce();
        }

        public override void DoCheck()
        {
            TrackingFallingForce();

            isGrounded = player.Core.Sensor.IsGroundDetect();
            сuгrentVelocityY = player.Core.Movement.CurrentVelocity.y;

            if (isLedgeDetected = player.Core.Sensor.IsHorizonalLedgCornerDetect(out Vector2 ledgeCorner))
                PlayerOnLedgeState.CornerPosition = ledgeCorner;

            if (isGrabWallDetected = player.Core.Sensor.IsGrabWallDetect())
                player.OnWallState.DetectedPos = player.Core.Sensor.WallHit.point;
        }

		public override void AnimationTrigger() 
            => player.Animator.Play(hashInAir);


        private void TrackingFallingForce() 
        {
            if (сuгrentVelocityY < fallingForce)
            {
                fallingForce = сuгrentVelocityY;
            }
        }

        private void ResetFallingForce() => fallingForce = default;

        #region Input
        private void OnJump()
        {
            if (!player.JumpState.CanJump()) return;

            stateMachine.ChangeState(player.JumpState);
        }

        private void OnAirDash()
        {
			if (!player.AirDashState.CanDash()) return;

            stateMachine.ChangeState(player.AirDashState);
        }
        #endregion
    }
}