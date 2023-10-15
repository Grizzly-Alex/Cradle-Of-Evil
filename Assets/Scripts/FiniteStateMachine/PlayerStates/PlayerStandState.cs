using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerStandState : PlayerOnGroundState
    {
        private readonly int hashIdle = Animator.StringToHash("IdleStand");
        private readonly int hashRun = Animator.StringToHash("RunStart");

        public PlayerStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        { 
        }

        public override void Enter()
        {
            base.Enter();
            
            player.Input.SitStandEvent += OnSit;
            player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnDash;

            player.SetColliderHeight(player.StandColiderHeight);

            if (player.Input.NormInputX != 0)
            {
                player.Animator.Play(hashRun);
            }
            else
            {
                player.Animator.Play(hashIdle);
            }
        }

        public override void Update()
        {
            base.Update();

            player.Core.Movement.Move(player.StandMoveSpeed, player.Input.NormInputX);
        }

        public override void Exit()
        {
            base.Exit();
            player.Input.SitStandEvent -= OnSit;
            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnDash;
        }

        #region Input
        private void OnSit()
        {
            stateMachine.ChangeState(player.SitStandState);
        }

        private void OnJump()
        {
            stateMachine.ChangeState(player.JumpState);            
        }

        private void OnDash()
        {
            stateMachine.ChangeState(player.GroundDashState);
        }
        #endregion
    }
}
