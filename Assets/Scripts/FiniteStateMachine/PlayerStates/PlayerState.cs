using Entities;
using Interfaces;
using UnityEngine;


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
            Debug.Log(this);
        }

        public virtual void LogicUpdate()
        {
            DoCheck();
        }

        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {
            player.PreviousState = this;
        }

        public virtual void DoCheck() 
        {
        }

        public virtual void AnimationFinishTrigger()
            => isAnimFinished = true;

        public virtual void AnimationTrigger()
        {
        }
    }
}