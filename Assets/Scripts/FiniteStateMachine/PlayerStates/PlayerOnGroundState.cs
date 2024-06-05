using Entities;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerOnGroundState : PlayerState
    {
        protected bool isGrounded; 
        protected abstract float ColiderHeight { get; }


        protected PlayerOnGroundState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }


        public override void Enter()
        {
            base.Enter();

            player.SetColliderHeight(ColiderHeight);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.NewCore.Physics.Flipping.FlipToDirection(player.Input.InputHorizontal);

			if (!isGrounded)
            {
                player.Core.Movement.ResetFreezePos();
                player.JumpState.DecreaseAmountOfJump();
				stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void DoCheck()
        {
            isGrounded = player.NewCore.Sensor.GroundDetector.IsGroundDetect();
        }
	}
}
