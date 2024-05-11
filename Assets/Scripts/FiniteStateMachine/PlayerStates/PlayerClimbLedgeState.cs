using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerClimbLedgeState : PlayerOnLedgeState
    {
        private Vector2 climbedPosition;
        //private bool isTouchingCeiling;
        protected override int AnimationHash => Animator.StringToHash("LedgeClimb");

        public PlayerClimbLedgeState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            climbedPosition = GetClimbedPosition();
            player.transform.position = climbedPosition;
            player.Core.VisualFx.ShadowIsActive(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAnimFinished)
            {
                stateMachine.ChangeState(player.StandState);               
            }
        }       

        public override void Exit()
        {
            base.Exit();

            //if (isTouchingCeiling) player.SetColliderHeight(player.Data.CrouchColiderHeight);
        }

        public override void DoCheck()
        {
            //isTouchingCeiling = IsLowCeiling();
        }

        public override void AnimationTrigger()
        {
            player.Core.VisualFx.ShadowIsActive(true);
            player.Core.VisualFx.CreateAnimationFX(DustType.Landing, climbedPosition, player.transform.rotation);
        }

        //private bool IsLowCeiling()
        //{
        //    float offset = Physics2D.defaultContactOffset;

        //    return Physics2D.Raycast(
        //        CornerPosition + (Vector2.up * offset) + (offset * player.Core.Movement.FacingDirection * Vector2.right),
        //        Vector2.up,
        //        player.Data.StandColiderHeight,
        //        player.Core.Sensor.TerrainLayer);
        //}

        private Vector2 GetClimbedPosition()
        {
            return new(CornerPosition.x + player.BodyCollider.size.x / 2 * player.Core.Movement.FacingDirection,
                CornerPosition.y + Physics2D.defaultContactOffset);
        }
    }
}
