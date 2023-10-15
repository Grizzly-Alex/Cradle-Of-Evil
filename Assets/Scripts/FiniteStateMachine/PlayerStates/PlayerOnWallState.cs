using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerOnWallState : PlayerBaseState
    {
        public Vector2 DetectedPos { private get; set; }

        private readonly int landingOnWall = Animator.StringToHash("LandingOnWall");
        private bool isGrabWallDetected;
        private bool isGrounded;
        private Vector2 holdPosition;

        public PlayerOnWallState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            player.JumpState.ResetAmountOfJumpsLeft();

            player.Core.Movement.SetVelocityZero();
            holdPosition.Set(DetectedPos.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset)
                * player.Core.Movement.FacingDirection, DetectedPos.y);

            player.transform.position = holdPosition;

            player.Animator.Play(landingOnWall);
        }

        public override void Update()
        {
            base.Update();

            player.transform.position = holdPosition;

            if (!isGrabWallDetected)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (isGrounded)
            {
                stateMachine.ChangeState(player.StandState);
            }          
        }

        public override void Exit()
        {
            base.Exit();
            player.Input.JumpEvent -= OnJump;
        }

        public override void DoCheck()
        {
            isGrabWallDetected = player.Core.Sensor.IsGrabWallDetect();
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }

        private void OnJump()
        {
            if (isAnimFinished)
                stateMachine.ChangeState(player.WallJumpState);
        }
    }
}
