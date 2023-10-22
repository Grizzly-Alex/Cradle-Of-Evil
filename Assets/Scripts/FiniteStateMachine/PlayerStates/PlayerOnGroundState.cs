using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerOnGroundState : PlayerBaseState
    {
        protected bool isGrounded;
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");       
        protected abstract float MoveSpeed {  get; }

        protected PlayerOnGroundState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
			player.Input.JumpEvent += OnJump;
			player.Input.DashEvent += OnSlide;
			
		}
        public override void Update()
        {
            base.Update();

            player.Core.Movement.FlipToMovement(player.Input.NormInputX);
            player.Animator.SetBool(hashIsMoving, player.Input.NormInputX != 0);
			player.Core.Movement.Move(MoveSpeed, player.Input.NormInputX);

			if (!isGrounded)
            {
                player.JumpState.DecreaseAmountOfJump();
				stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();

			player.Input.JumpEvent -= OnJump;
			player.Input.DashEvent -= OnSlide;
        }

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }

        protected abstract void OnJump();
		protected abstract void OnSlide();
	}
}
