using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerInAirState : PlayerState
    {
        private readonly int hashVelocityY = Animator.StringToHash("velocityY");
        private readonly int hashInAir = Animator.StringToHash("InAirState");

        private bool isOneWayPlatform;
        private bool isPlatform;
        private bool isLedgeDetected;
        private bool isGrabWallDetected;
        private bool isFalling;
        private float fallingForce;

        public PlayerInAirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnAirDash;

            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.Data.StandColiderHeight);

            player.Animator.Play(hashInAir);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isOneWayPlatform || isPlatform)
            {               
                player.LandingState.LandingForce = fallingForce;
                stateMachine.ChangeState(player.LandingState);
            }
            else if (isLedgeDetected && isFalling) 
            {
				stateMachine.ChangeState(player.HangOnLedgeState);
            }
            else if (isGrabWallDetected && isFalling)
            {
				stateMachine.ChangeState(player.OnWallState);
            }

            player.Animator.SetFloat(hashVelocityY, player.Core.Movement.CurrentVelocity.y);
            player.Core.Movement.FlipToDirection(player.Input.InputHorizontal);
        }

        public override void PhysicsUpdate() 
        {
            base.PhysicsUpdate();

            player.Core.Movement.SetVelocityX(player.Input.InputHorizontal * player.Data.InAirMoveSpeed);
        }

        public override void Exit()
        {
            base.Exit();

            isLedgeDetected = false;
            isGrabWallDetected = false;
            isOneWayPlatform = false;
            isPlatform = false;

            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnAirDash;

            player.JumpState.DisableDoubleJumpFX();
            
            ResetFallingForce();
        }

        public override void DoCheck()
        {
            TrackingFallingForce();

            isFalling = player.Core.Movement.CurrentVelocity.y <= (float)default;

            isPlatform = player.Core.Sensor.IsPlatformDetect();

            if (isFalling) 
            {
                isOneWayPlatform = player.Core.Sensor.IsOneWayPlatformDetect();
            }
                
            if (player.HangOnLedgeState.CanHang())
            {
                if (isLedgeDetected = player.Core.Sensor.GetDetectedLedgeCorner(out Vector2 ledgeCorner))
                    PlayerOnLedgeState.CornerPosition = ledgeCorner;
            }
          
            if (isGrabWallDetected = player.Core.Sensor.GetDetectedGrabWallPosition(out Vector2 wallPosition))
                PlayerOnWallState.DetectedPosition = wallPosition;
        }

		public override void AnimationTrigger() 
            => player.Animator.Play(hashInAir);


        private void TrackingFallingForce() 
        {
            if (player.Core.Movement.CurrentVelocity.y < fallingForce)
            {
                fallingForce = player.Core.Movement.CurrentVelocity.y;
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