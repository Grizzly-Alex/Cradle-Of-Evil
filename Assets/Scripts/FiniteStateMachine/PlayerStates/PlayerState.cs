using Entities;
using Interfaces;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerState : IState
    {
        protected StateMachine stateMachine;
        protected Player player;
        protected bool isAnimFinished;

        public PlayerState(StateMachine stateMachine, Player player)
        {
            this.stateMachine = stateMachine;
            this.player = player;
        }

        public virtual void Enter()
        {
            isAnimFinished = false;    
        }

        public virtual void Update()
        {
            DoCheck();
        }

        public virtual void Exit()
        {
            player.PreviousState = this;           
        }

        public abstract void DoCheck();

        public virtual void AnimationFinishTrigger()
            => isAnimFinished = true;

        public virtual void AnimationTrigger()
        {
        }
    }
}