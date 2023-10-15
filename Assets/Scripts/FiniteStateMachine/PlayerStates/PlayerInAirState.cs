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

        public PlayerInAirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            startTime = Time.time;
            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.StandColiderHeight);
            DefaultJumpCount();

            player.Animator.Play(
                player.JumpState.AmountOfJumpsLeft != 0
                ? hashInAir
                : player.JumpState.GetHashAnimDoubleJump());           
        }

        public override void Update()
        {
            base.Update();

            player.Animator.SetFloat(hashVelocityY, сuгrentVelocityY);
            player.Core.Movement.FlipToMovement(player.Input.NormInputX);
            player.Core.Movement.SetVelocityX(player.Input.NormInputX * player.InAirMoveSpeed);

            if (isGrounded)
            {
                player.LandingState.LandingForce = fallingForce;
                stateMachine.ChangeState(player.LandingState);
            }
            else if (isLedgeDetected && Cooldown(player.GrabLedgeCooldown) && сuгrentVelocityY <= 0.0f) 
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

        private void TrackingFallingForce()
        {
            if (сuгrentVelocityY < 0.0f)
                fallingForce = сuгrentVelocityY;           
        }

        private bool Cooldown(float finishTime) => Time.time >= finishTime + startTime;

        private void ResetFallingForce() => fallingForce = default;

        private void DefaultJumpCount()
        {
            if (player.JumpState.AmountOfJumpsLeft != player.AmountOfJump) return;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        private void OnJump()
        {
            if (player.JumpState.CanJum()) stateMachine.ChangeState(player.JumpState);
        }
    }
}