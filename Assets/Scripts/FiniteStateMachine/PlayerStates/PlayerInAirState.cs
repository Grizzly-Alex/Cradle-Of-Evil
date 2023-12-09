using Entities;
using Pool.ItemsPool;
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
        public bool UseDoubleJump {  get; set; }

        public PlayerInAirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

			player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnDash;
                       
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
            else if (isLedgeDetected && сuгrentVelocityY <= 0.0f) 
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
            if (isLedgeDetected)
                player.States.LedgeClimb.DetectedPos = player.transform.position;

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

        private void ResetFallingForce() => fallingForce = default;

        private void OnJump()
        {
            if (!player.States.Jump.CanJump()) return;

            stateMachine.ChangeState(player.States.Jump);
        }

        private void OnDash()
        {
			if (!player.States.Dash.CanDash()) return;

            stateMachine.ChangeState(player.States.Dash);
            player.Core.VisualFx.CreateDust(DustType.Dash, player.BodyCollider.bounds.center, player.transform.rotation);
        }
    }
}