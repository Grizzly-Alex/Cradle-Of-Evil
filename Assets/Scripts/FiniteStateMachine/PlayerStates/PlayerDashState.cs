using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerDashState : PlayerAbilityState
    {
        private readonly int hashDash = Animator.StringToHash("Dashing");
        private readonly int hashIsDashing = Animator.StringToHash("isDashing");
		private readonly int amountOfDash = 1;
        private bool isDashing;
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
            isDashing = true;
			player.Animator.SetBool(hashIsDashing, true);
			player.Animator.Play(hashDash);
        }

        public override void Update()
        {
            base.Update();

            if (Time.time >= finishTime)
            {
                if (isDashing)
                {
                    player.Core.Movement.SetVelocityZero();
                    isDashing = false;
					player.Animator.SetBool(hashIsDashing, false);
				}

                if(isAnimFinished) 
                {
                    isAbilityDone = true;
                }
            }
            else
            {
                player.Core.Movement.SetVelocity(player.Data.DashSpeed, new Vector2(1, 0), player.Core.Movement.FacingDirection);
            }
        }

        public override void Exit()
        {
            base.Exit();

			DecreaseAmountOfDash();
		}

        public bool CanDash() => amountOfDashLeft > 0;
		public void ResetAmountOfDash() => amountOfDashLeft = amountOfDash;
		public void DecreaseAmountOfDash() => amountOfDashLeft--;
	}
}
