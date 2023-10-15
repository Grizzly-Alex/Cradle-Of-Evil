using Entities;
using Interfaces;
using UnityEngine;

namespace FiniteStateMachine
{
    public abstract class PlayerBaseState : IState
    {
        protected Player player;
        protected StateMachine stateMachine;
        protected bool isAnimFinished;

        public PlayerBaseState(StateMachine stateMachine, Player player)
        {
            this.stateMachine = stateMachine;
            this.player = player;
        }

        public virtual void Enter()
        {
            isAnimFinished = false;          
            DoCheck();
        }

        public virtual void Update()
        {
            DoCheck();
        }

        public virtual void Exit()
        {

        }

        public abstract void DoCheck();

        public virtual void AnimationFinishTrigger()
            => isAnimFinished = true;

        public virtual void AnimationTrigger()
        {
        }
    }
}
