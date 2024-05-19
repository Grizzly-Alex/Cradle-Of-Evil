using Entities;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSitStandState : PlayerState
    {
        private readonly int hashStandUp = Animator.StringToHash("StandUp");
        private readonly int hashSitDown = Animator.StringToHash("SitDown");

        private bool isGrounded;
        private Action update;

        public PlayerSitStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            player.Core.Movement.SetVelocityZero();
            player.Core.Movement.FreezePosOnSlope();

            switch (player.PreviousState)
            {
                case PlayerCrouchState:
                    player.Animator.Play(hashStandUp);
                    update = () => stateMachine.ChangeState(player.StandState);
                    break;
                case PlayerStandState:
                    player.Animator.Play(hashSitDown);
                    update = () => stateMachine.ChangeState(player.CrouchState);
                    break;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAnimFinished)
            {
                update.Invoke();
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

            player.Input.JumpEvent -= OnJump;
        }

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsPlatformDetect()
                || player.Core.Sensor.IsOneWayPlatformDetect();
        }

        private void OnJump()
        {
            if (player.Input.InputVertical == Vector2.down.y)
            {
                if (!player.Core.Sensor.IsOneWayPlatformDetect()) return;
                player.Core.Movement.LeaveOneWayPlatform();
            }
            else
            {
                stateMachine.ChangeState(player.JumpState);
            }
        }
    }
}
