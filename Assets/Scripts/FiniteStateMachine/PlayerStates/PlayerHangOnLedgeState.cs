using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerHangOnLedgeState : PlayerOnLedgeState
    {
        protected override int AnimationHash => Animator.StringToHash("LedgeGrab");

        private bool isHanging;
        private bool canHang;

        public PlayerHangOnLedgeState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
            canHang = true;
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnClimb;

            player.AirDashState.ResetAmountOfDash();
            player.JumpState.ResetAmountOfJump();
            player.Core.Movement.SetVelocityZero();

            player.Core.VisualFx.CreateAnimationFX(
                DustType.Tiny,
                CornerPosition,
                new Quaternion() { y = player.Core.Movement.FacingDirection == Vector2.left.x ? 0 : 180 });

            player.transform.position = GetHangingPosition();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.Input.InputVertical == Vector2.up.y && isHanging)
            {
                stateMachine.ChangeState(player.ClimbLedgeState);
            }
            else if (player.Input.InputVertical == Vector2.down.y && isHanging)
            {
                canHang = false;
                player.Core.Movement.SetVelocityY(0.1f);
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            isHanging = false;         
            player.Input.JumpEvent -= OnClimb;
        }

        public override void AnimationTrigger()
            => isHanging = true;

        public bool CanHang()
        {
            if (!player.Core.Sensor.IsLedgeDetect())
            {
                canHang = true;
            }
            return canHang;
        }

        private Vector2 GetHangingPosition()
        {
            return new(CornerPosition.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * player.Core.Movement.FacingDirection,
                CornerPosition.y - player.BodyCollider.size.y + Physics2D.defaultContactOffset);
        }

        #region Input
        private void OnClimb()
        {
            if (!isHanging) return;

            stateMachine.ChangeState(player.ClimbLedgeState);
        }
        #endregion

    }
}
