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

        public override void LogicUpdate()
        {
            base.LogicUpdate();

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
            isGrounded = sensorCore.GroundDetector.IsPlatformDetect() 
                || sensorCore.GroundDetector.IsOneWayPlatformDetect();
        }
    }
}