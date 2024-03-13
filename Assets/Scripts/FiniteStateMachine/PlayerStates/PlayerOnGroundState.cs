using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerOnGroundState : PlayerState
    {
        protected bool isGrounded;
        protected bool isCrouching;
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");       

        protected abstract float MoveSpeed { get; }
        protected abstract int HashIdle { get; }
        protected abstract int HashMove { get; }
        protected abstract float ColiderHeight { get; }


        protected PlayerOnGroundState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }


        public override void Enter()
        {
            base.Enter();

			player.Input.JumpEvent += OnJump;
			player.Input.DashEvent += OnSlide;

            player.SetColliderHeight(ColiderHeight);
            player.Animator.Play(player.Input.NormInputX != 0 ? HashMove : HashIdle);

        }
        public override void Update()
        {
            base.Update();

            player.Core.Movement.FlipToMovement(player.Input.NormInputX);
            player.Animator.SetBool(hashIsMoving, player.Input.NormInputX != 0);
			player.Core.Movement.Move(MoveSpeed, player.Input.NormInputX);

			if (!isGrounded)
            {
                if (isCrouching)
                {
                    isCrouching = false;
                    player.SetColliderHeight(player.Data.StandColiderHeight);
                }
                player.Core.Movement.ResetFreezePos();
                player.JumpState.DecreaseAmountOfJump();
				stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();

			player.Input.JumpEvent -= OnJump;
			player.Input.DashEvent -= OnSlide;

            player.LastOnGroundState = this;
        }

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }

        protected abstract void OnJump();
		protected abstract void OnSlide();
	}
}
