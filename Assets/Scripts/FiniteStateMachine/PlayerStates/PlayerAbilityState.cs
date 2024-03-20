using Entities;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerAbilityState : PlayerState
    {
        protected bool isGrounded;
        protected bool isAbilityDone;

        protected PlayerAbilityState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            isAbilityDone = false;
        }

        public override void Update()
        {
            base.Update();

            if (isAbilityDone) 
            {
                if (isGrounded)
                {
                    stateMachine.ChangeState(player.StandState);                   
                }
                else
                {
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }
    }
}