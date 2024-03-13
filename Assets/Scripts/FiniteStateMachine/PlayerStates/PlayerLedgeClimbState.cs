using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerLedgeClimbState : PlayerState
    {
        public PlayerLedgeClimbState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public Vector2 CornerPosition { get; set; }
        private Vector2 startPos;
        private Vector2 stopPos;
        private bool isHanging;
        private bool isClimbing;
        private bool isTouchingCeiling;
        private readonly int hashLedgeGrab = Animator.StringToHash("LedgeGrab");
        private readonly int isClimbingHash = Animator.StringToHash("isClimbing");

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            player.DashState.ResetAmountOfDash();
            player.JumpState.ResetAmountOfJump();

            player.Core.VisualFx.CreateDust(
                DustType.Tiny,
                CornerPosition,
                new Quaternion() { y = player.Core.Movement.FacingDirection == -1 ? 0 : 180 });

            startPos.Set(
                CornerPosition.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * player.Core.Movement.FacingDirection,
                CornerPosition.y - player.BodyCollider.size.y + Physics2D.defaultContactOffset);
            stopPos.Set(
                CornerPosition.x + player.BodyCollider.size.x * player.Core.Movement.FacingDirection,
                CornerPosition.y + Physics2D.defaultContactOffset);

            player.transform.position = startPos;
            player.Core.Movement.FreezePosY();

            player.Animator.Play(hashLedgeGrab);
        }

        public override void Update()
        {
            base.Update();

            if (isAnimFinished)
            {
                if (isTouchingCeiling)
                {
                    stateMachine.ChangeState(player.CrouchState);
                }
                else
                {
                    stateMachine.ChangeState(player.SitStandState);
                }
            }
            else
            {
                if (player.Core.Movement.FacingDirection == player.Input.NormInputX && isHanging && !isClimbing)
                {
                    isClimbing = true;
                    player.Animator.SetBool(isClimbingHash, true);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJump;

            player.Core.Movement.ResetFreezePos();

            isHanging = false;
            player.Animator.SetBool(isClimbingHash, false);

            if (isTouchingCeiling) player.SetColliderHeight(player.Data.CrouchColiderHeight);

            if (isClimbing)
            {
                player.Core.VisualFx.CreateDust(DustType.Landing, stopPos, player.transform.rotation);
                player.transform.position = stopPos;
                isClimbing = false;
            }
        }

        public override void DoCheck()
        {
            isTouchingCeiling = IsLowCeiling();
        }

        private void OnJump()
        {
            if (isClimbing && isHanging) return;

            stateMachine.ChangeState(player.JumpState);

            player.Core.VisualFx.CreateDust(
                DustType.JumpFromWall,
                CornerPosition,
                player.transform.rotation);
        }

        public override void AnimationTrigger() 
            => isHanging = true;

        private bool IsLowCeiling()
        {
            float offset = Physics2D.defaultContactOffset;

            return Physics2D.Raycast(
                CornerPosition + (Vector2.up * offset) + (offset * player.Core.Movement.FacingDirection * Vector2.right),
                Vector2.up,
                player.Data.StandColiderHeight,
                player.Core.Sensor.PlatformsLayer);
        }
    }
}
