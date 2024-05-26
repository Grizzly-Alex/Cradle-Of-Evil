using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerHangOnGirderState : PlayerHangState
    {
        private readonly float offsetPosition = 0.06f;

        public PlayerHangOnGirderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
                      
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;
            player.JumpState.ResetAmountOfJump();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            player.Input.JumpEvent -= OnJump;
        }

        protected override Vector2 GetHangingPosition()
        {
            return new(GrapPosition.x - (player.BodyCollider.size.x / 2 + offsetPosition) * player.Core.Movement.FacingDirection,
                GrapPosition.y - player.BodyCollider.size.y);
        }

        #region Input
        private void OnJump()
        {
            if (!isHanging) return;

            stateMachine.ChangeState(player.JumpState);
        }
        #endregion
    }
}
