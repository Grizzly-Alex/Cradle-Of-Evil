using System;
using UnityEngine;

public sealed class PlayerTransitionState : PlayerBaseState 
{
    private readonly int _hashAnimation;
    private readonly PlayerBaseState _nextState;
    
    public PlayerTransitionState(PlayerStateMachine stateMachine, string animimation, PlayerBaseState nextState) : base(stateMachine)
    {
        _hashAnimation = Animator.StringToHash(animimation);
        _nextState = nextState;    
    }

    public override void Enter()
    {
        base.Enter(); 
     
        core.Movement.SetVelocityZero();
        
        animator.Play(_hashAnimation);  

        core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeAll);             
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        { 
            stateMachine.SwitchState(_nextState);     
        }

        if(!isGrounded && core.Movement.CurrentVelocity.y < 0.0f)
        { 
            stateMachine.JumpingState.DecreaseAmountOfJumpsLeft();
            
            stateMachine.SwitchState(stateMachine.FallingState);        
        }     
    }

    public override void Exit()
    {   
        base.Exit();

        core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeRotation); 
    }
}
