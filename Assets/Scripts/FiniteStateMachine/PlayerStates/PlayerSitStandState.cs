using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSitStandState : PlayerBaseState
    {
        private readonly int hashStandUp = Animator.StringToHash("StandUp");
        private readonly int hashSitDown = Animator.StringToHash("SitDown");

        private bool isGrounded;

        public PlayerSitStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.Core.Movement.SetVelocityZero();
            player.Core.Movement.FreezePosOnSlope();

            switch (stateMachine.PreviousState)
            {
                case PlayerCrouchState: player.Animator.Play(hashStandUp); break;
                case PlayerStandState: player.Animator.Play(hashSitDown); break;
                case PlayerLedgeClimbState: player.Animator.Play(hashStandUp); break;
            }
        }

        public override void Update()
        {
            base.Update();

            if (isAnimFinished)
            {
                switch (stateMachine.PreviousState)
                {
                    case PlayerCrouchState: stateMachine.ChangeState(player.StandState); break;
                    case PlayerStandState: stateMachine.ChangeState(player.CrouchState); break;
                    case PlayerLedgeClimbState: stateMachine.ChangeState(player.StandState); break;
                }
            }

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
