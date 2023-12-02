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
				player.States.Landing.LandingForce = fallingForce;
                stateMachine.ChangeState(player.States.Landing);
            }
            else if (isLedgeDetected && Cooldown(player.Data.GrabLedgeCooldown) && сuгrentVelocityY <= 0.0f) 
            {
				stateMachine.ChangeState(player.States.LedgeClimb);
            }
            else if (isGrabWallDetected && сuгrentVelocityY <= 0.0f)
            {
				stateMachine.ChangeState(player.States.OnWall);
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

            isGrabWallDetected = player.Core.Sensor.IsGrabWallDetect();
            if (isGrabWallDetected)
                player.States.OnWall.DetectedPos = player.Core.Sensor.WallHit.point;
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
				player.Animator.Play(player.States.Jump.GetDoubleJumpHashAnim());
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

        private bool Cooldown(float finishTime) => Time.time >= finishTime + startTime;

        private void ResetFallingForce() => fallingForce = default;

        private void OnJump()
        {
            if (player.States.Jump.CanJump())
                stateMachine.ChangeState(player.States.Jump);          
        }

        private void OnDash()
        {
			if (player.States.Dash.CanDash())
				stateMachine.ChangeState(player.States.Dash);
        }
    }
}