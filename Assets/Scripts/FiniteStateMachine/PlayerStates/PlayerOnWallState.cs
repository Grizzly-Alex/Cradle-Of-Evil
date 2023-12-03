using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerOnWallState : PlayerBaseState
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

            player.Input.JumpEvent += OnJump;

			player.States.Dash.ResetAmountOfDash();
			player.States.Jump.ResetAmountOfJump();

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
                stateMachine.ChangeState(player.States.Stand);
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
            if (isAnimFinished)
                stateMachine.ChangeState(player.States.Jump);
        }
    }
}
