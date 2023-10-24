using Entities;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSitStandState : PlayerBaseState
    {
        private readonly int hashStandUp = Animator.StringToHash("StandUp");
        private readonly int hashSitDown = Animator.StringToHash("SitDown");

        private bool isGrounded;
        private Action changeState;

        public PlayerSitStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.Core.Movement.SetVelocityZero();
            player.Core.Movement.FreezePosOnSlope();

            switch (player.PreviousState)
            {
                case PlayerCrouchState or PlayerLedgeClimbState:
                    player.Animator.Play(hashStandUp);
                    changeState = () => stateMachine.ChangeState(player.StandState);
                    break;
                case PlayerStandState:
                    player.Animator.Play(hashSitDown);
                    changeState = () => stateMachine.ChangeState(player.CrouchState);
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            if (isAnimFinished)
            {
                changeState.Invoke();
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
