using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private readonly int hashVelocityY = Animator.StringToHash("velocityY");
        private readonly int hashInAir = Animator.StringToHash("InAirState");
        private float сuгrentVelocityY;
        private float finishTime;

        public PlayerWallJumpState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.WallJumpTime;
            player.Animator.Play(hashInAir);
            player.Core.Movement.Flip();
            player.Core.Movement.SetVelocity(player.Data.FirstJumpForce, new Vector2(1,2), player.Core.Movement.FacingDirection);
        }

        public override void Update()
        {
            base.Update();

            player.Animator.SetFloat(hashVelocityY, сuгrentVelocityY);

            if (Time.time >= finishTime) isAbilityDone = true;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void DoCheck()
        {
            base.DoCheck();
            сuгrentVelocityY = player.Core.Movement.CurrentVelocity.y;
        }
    }
}
