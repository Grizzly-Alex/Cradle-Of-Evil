using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerOnGroundState : PlayerBaseState
    {
        protected bool isGrounded;
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");       

        protected PlayerOnGroundState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();

            player.Core.Movement.FlipToMovement(player.Input.NormInputX);
            player.Animator.SetBool(hashIsMoving, player.Input.NormInputX != 0);

            if (!isGrounded)
            {
                player.Core.Movement.ResetFreezePos();
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }
    }
}
