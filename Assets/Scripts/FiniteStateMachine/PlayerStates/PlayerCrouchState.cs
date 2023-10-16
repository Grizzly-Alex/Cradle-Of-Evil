using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerCrouchState : PlayerOnGroundState
    {
        private readonly int hashIdle = Animator.StringToHash("IdleCrouch");
        private bool isTouchedRoof;

        public PlayerCrouchState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.SitStandEvent += OnStand;
            player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnDash;

            player.SetColliderHeight(player.Data.CrouchColiderHeight);
            player.Animator.Play(hashIdle);
        }

        public override void Update()
        {
            base.Update();

            player.Core.Movement.Move(player.Data.CrouchMoveSpeed, player.Input.NormInputX);
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.SitStandEvent -= OnStand;
            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnDash;
        }

        public override void DoCheck()
        {
            base.DoCheck();
            isTouchedRoof = player.Core.Sensor.IsCellingDetect();
        }

        #region Input
        private void OnStand()
        {
            if (!isTouchedRoof) stateMachine.ChangeState(player.SitStandState);            
        }

        private void OnJump()
        {
            if (!isTouchedRoof) stateMachine.ChangeState(player.JumpState);
        }

        private void OnDash()
        {
            stateMachine.ChangeState(player.GroundDashState);
        }
        #endregion
    }
}