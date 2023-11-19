using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerLedgeClimbState : PlayerBaseState
    {
        public PlayerLedgeClimbState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public Vector2 DetectedPos { private get; set; }

        private Vector2 startPos;
        private Vector2 stopPos;
        private Vector2 cornerPos;
        private bool isHanging;
        private bool isClimbing;
        private bool isTouchingCeiling;
        private readonly int hashLedgeGrab = Animator.StringToHash("LedgeGrab");
        private readonly int isClimbingHash = Animator.StringToHash("isClimbing");

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

			player.States.Dash.ResetAmountOfDash();
			player.States.Jump.ResetAmountOfJump();

			player.Core.Movement.SetVelocityZero();

            player.transform.position = DetectedPos;
            cornerPos = GetCornerOfLedge();
            startPos.Set(
                cornerPos.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * player.Core.Movement.FacingDirection,
                cornerPos.y + Mathf.Abs(player.BodyCollider.offset.y) - player.BodyCollider.size.y / 2);
            stopPos.Set(
                cornerPos.x + player.BodyCollider.size.x * player.Core.Movement.FacingDirection,
                cornerPos.y + Mathf.Abs(player.BodyCollider.offset.y) + player.BodyCollider.size.y / 2 + Physics2D.defaultContactOffset);

            player.transform.position = startPos;
            player.Animator.Play(hashLedgeGrab);
        }
        public override void Update()
        {
            base.Update();

            if (isAnimFinished)
            {
                if (isTouchingCeiling)
                {
                    stateMachine.ChangeState(player.States.Crouch);
                }
                else
                {
                    stateMachine.ChangeState(player.States.SitStand);
                }
            }
            else
            {
                player.Core.Movement.SetVelocityZero();
                player.transform.position = startPos;

                if (player.Core.Movement.FacingDirection == player.Input.NormInputX && isHanging && !isClimbing)
                {
                    isClimbing = true;
                    player.Animator.SetBool(isClimbingHash, true);
                }
                else if (player.Input.NormInputY == -1 && isHanging && !isClimbing)
                {
                    stateMachine.ChangeState(player.States.InAir);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJump;

            isHanging = false;
            player.Animator.SetBool(isClimbingHash, false);

            if (isTouchingCeiling) player.SetColliderHeight(player.Data.CrouchColiderHeight);

            if (isClimbing)
            {
                player.transform.position = stopPos;
                isClimbing = false;
            }
        }

        public override void DoCheck()
        {
            isTouchingCeiling = CheckForSpace();
        }

        private void OnJump()
        {
            if (!isClimbing && isHanging) 
                stateMachine.ChangeState(player.States.Jump);
        }

        public override void AnimationTrigger() => isHanging = true;

        private bool CheckForSpace()
        {
            return Physics2D.Raycast(
                cornerPos + (Vector2.up * Physics2D.defaultContactOffset) + (Physics2D.defaultContactOffset * player.Core.Movement.FacingDirection * Vector2.right),
                Vector2.up,
                player.Data.StandColiderHeight,
                player.Core.Sensor.PlatformsLayer);
        }

        private Vector2 GetCornerOfLedge()
        {
            float positionX = player.Core.Movement.FacingDirection * player.Core.Sensor.WallHit.distance + player.Core.Sensor.WallSensor.position.x;

            RaycastHit2D hitDown = Physics2D.Raycast(
                new Vector2(positionX, player.Core.Sensor.LedgeHorizontalSensor.position.y),
                Vector2.down,
                player.Core.Sensor.LedgeHorizontalSensor.position.y - player.Core.Sensor.WallSensor.position.y,
                player.Core.Sensor.PlatformsLayer);

            float positionY = player.Core.Sensor.LedgeHorizontalSensor.position.y - hitDown.distance;

            return new Vector2(positionX, positionY);
        }
    }
}
