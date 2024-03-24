using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerHangOnLedgeState : PlayerOnLedgeState
    {
        private bool isHanging;
        protected override int AnimationHash => Animator.StringToHash("LedgeGrab");


        public PlayerHangOnLedgeState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }


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

            player.transform.position = GetHangingPosition();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.Core.Movement.FacingDirection == player.Input.NormInputX && isHanging)
            {
                stateMachine.ChangeState(player.ClimbLedgeState);              
            }           
        }

        public override void Exit()
        {
            base.Exit();

            isHanging = false;         
            player.Input.JumpEvent -= OnJump;
        }

        public override void AnimationTrigger()
            => isHanging = true;

        #region Input
        private void OnJump()
        {
            if (!isHanging) return;

            stateMachine.ChangeState(player.JumpState);
        }
        #endregion

        private Vector2 GetHangingPosition()
        {
            return new(CornerPosition.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * player.Core.Movement.FacingDirection,
                CornerPosition.y - player.BodyCollider.size.y + Physics2D.defaultContactOffset);
        }
    }
}
