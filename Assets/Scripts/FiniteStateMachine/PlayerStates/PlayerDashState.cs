using Assets.Scripts;
using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerDashState : PlayerAbilityState
    {
        private readonly int hashDash = Animator.StringToHash("Dashing");
		private readonly int amountOfDash = 1;
		private int amountOfDashLeft;
        private float finishTime;

		public PlayerDashState(StateMachine stateMachine, Player player) : base(stateMachine, player)
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
        }

        public override void Update()
        {
            base.Update();

            if (Time.time >= finishTime)
            {
                isAbilityDone = true;
            }
            else
            {
                AfterImagePoolTest.Instance.GetFromPool();
                //player.Core.VisualFx.CreateAfterImage();
                player.Core.Movement.SetVelocity(player.Data.DashSpeed, new Vector2(1, 0), player.Core.Movement.FacingDirection);
            }
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
