using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerDashState : PlayerAbilityState
    {
        private readonly int hashDash = Animator.StringToHash("Dash");
        private float finishTime;
        private bool isGrabWall;

        public PlayerDashState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.DashTime;
            player.Animator.Play(hashDash);
        }

        public override void Update()
        {
            base.Update();

            if (isGrounded || isGrabWall || Time.time >= finishTime)
            {
                isAbilityDone = true;
            }
            else
            {
                player.Core.Movement.SetVelocity(player.Data.DashSpeed, new Vector2(1, 0), player.Core.Movement.FacingDirection);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void DoCheck()
        {
            base.DoCheck();

            isGrabWall = player.Core.Sensor.IsGrabWallDetect();
        }
    }
}
