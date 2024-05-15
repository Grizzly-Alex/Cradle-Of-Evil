using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerAirDashState : PlayerAbilityState
    {
        private readonly int hashDash = Animator.StringToHash("AirDashing");
		private readonly int amountOfDash = 1;
		private int amountOfDashLeft;
        private float finishTime;

		public PlayerAirDashState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
            amountOfDashLeft = amountOfDash;
        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.DashTime;
            player.Core.Movement.FreezePosY();
            player.Core.Movement.GravitationOff();
			player.Animator.Play(hashDash);
            player.Core.VisualFx.CreateAnimationFX(DustType.Dash, player.BodyCollider.bounds.center, player.transform.rotation);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Time.time >= finishTime)
            {
                isAbilityDone = true;
            }
            else
            {
                player.Core.VisualFx.CreateAfterImage(0.6f);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            player.Core.Movement.SetVelocity(player.Data.DashSpeed, new Vector2(1, 0), player.Core.Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

			DecreaseAmountOfDash();
            player.Core.Movement.SetVelocityZero();
            player.Core.Movement.ResetFreezePos();
			player.Core.Movement.GravitationOn();
		}

        public bool CanDash() => amountOfDashLeft > 0;
		public void ResetAmountOfDash() => amountOfDashLeft = amountOfDash;
		public void DecreaseAmountOfDash() => amountOfDashLeft--;
	}
}
