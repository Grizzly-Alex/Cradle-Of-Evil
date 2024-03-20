using Entities;
using Pool.ItemsPool;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerJumpState : PlayerAbilityState
    {
        private readonly int hashDoubleJump = Animator.StringToHash("DoubleJump");
		private readonly int hashVelocityY = Animator.StringToHash("velocityY");
		private readonly int hashInAir = Animator.StringToHash("InAirState");
		private readonly int amountOfJump = 2;
		private int amountOfJumpLeft;
		private float finishTime;
		private Action jumpUpdate;

        public PlayerJumpState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			amountOfJumpLeft = amountOfJump;
		}

        public override void Enter()
        {
            base.Enter();

            if (player.PreviousState is PlayerOnGroundState or PlayerLandingState) 
			{
                player.Core.VisualFx.CreateDust(
					DustType.JumpFromGround,
					player.Core.Sensor.GroundHit.point,
					player.transform.rotation);

                player.Core.Movement.SetVelocityY(player.Data.JumpForce);
                jumpUpdate = UpdateJump;
			}
			else if (player.PreviousState is PlayerOnWallState)
			{
                player.Core.VisualFx.CreateDust(DustType.JumpFromWall,
				new Vector2()
				{
					x = player.Core.Movement.FacingDirection != 1
						? player.BodyCollider.bounds.min.x
						: player.BodyCollider.bounds.max.x,
					y = player.BodyCollider.bounds.min.y,
				},
                player.transform.rotation, true);

                finishTime = Time.time + player.Data.WallJumpTime;
				player.Animator.Play(hashInAir);
				player.Core.Movement.Flip();
				player.Core.Movement.SetVelocity(
					player.Data.JumpForce,
					new Vector2(1, 2),
					player.Core.Movement.FacingDirection);
                jumpUpdate = UpdateJumpFromWall;
			}
            else if (player.PreviousState is PlayerHangOnLedgeState)
            {
                player.Core.VisualFx.CreateDust(DustType.JumpFromWall, 
					PlayerOnLedgeState.CornerPosition,
					player.transform.rotation, true);

                finishTime = Time.time + player.Data.WallJumpTime;
                player.Animator.Play(hashInAir);
                player.Core.Movement.Flip();
                player.Core.Movement.SetVelocity(
                    player.Data.JumpForce,
                    new Vector2(1, 2),
                    player.Core.Movement.FacingDirection);
                jumpUpdate = UpdateJumpFromWall;
            }
            else if (player.PreviousState is PlayerInAirState)
			{
                player.Core.Movement.SetVelocityY(player.Data.DoubleJumpForce);
                jumpUpdate = UpdateJump;
			}
        }

        public override void Update()
        {
            base.Update();

            jumpUpdate.Invoke();
        }

        public override void Exit()
        {
            base.Exit();

            DecreaseAmountOfJump();
		}

		#region Update
		private void UpdateJump()
		{
			if (!isGrounded) isAbilityDone = true;
		}

		private void UpdateJumpFromWall()
		{			
			player.Animator.SetFloat(hashVelocityY, player.Core.Movement.CurrentVelocity.y);
			if (Time.time >= finishTime) isAbilityDone = true;
		}
		#endregion

		public void ResetAmountOfJump() => amountOfJumpLeft = amountOfJump;
		public void DecreaseAmountOfJump() => amountOfJumpLeft--;
		public bool CanJump() => amountOfJumpLeft > 0;
		public int GetDoubleJumpHashAnim() => hashDoubleJump;
	}
}