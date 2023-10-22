using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerInAirState : PlayerBaseState
    {
        private readonly int hashVelocityY = Animator.StringToHash("velocityY");
        private readonly int hashInAir = Animator.StringToHash("InAirState");

        private bool isGrounded;
        private bool isLedgeDetected;
        private bool isGrabWallDetected;
        private float сuгrentVelocityY;
        private float fallingForce;
        private float startTime;
        public bool UseDoubleJump {  get; set; }

        public PlayerInAirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

			player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnDash;

            startTime = Time.time;
                       
            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.Data.StandColiderHeight);

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
            else if (isLedgeDetected && Cooldown(player.Data.GrabLedgeCooldown) && сuгrentVelocityY <= 0.0f) 
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

            isLedgeDetected = player.Core.Sensor.IsLedgeHorizontalDetect();
            if (isLedgeDetected) 
                player.LedgeClimbState.DetectedPos = player.transform.position;

            isGrabWallDetected = player.Core.Sensor.IsGrabWallDetect();
            if (isGrabWallDetected)
                player.OnWallState.DetectedPos = player.Core.Sensor.WallHit.point;
        }

		public override void AnimationTrigger() => player.Animator.Play(hashInAir);

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
            if (сuгrentVelocityY < 0.0f)
                fallingForce = сuгrentVelocityY;           
        }

        private bool Cooldown(float finishTime) => Time.time >= finishTime + startTime;

        private void ResetFallingForce() => fallingForce = default;

        private void OnJump()
        {
            if (player.JumpState.CanJump())
                stateMachine.ChangeState(player.JumpState);          
        }

        private void OnDash()
        {
            stateMachine.ChangeState(player.DashState);
        }
    }
}