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
     
        playerSm.Core.Movement.SetVelocityZero();
        
        playerSm.Animator.Play(_hashAnimation);  

        playerSm.Core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeAll);             
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAnimationFinished)
        { 
            playerSm.SwitchState(_nextState);     
        }

        if(!_isGrounded && playerSm.Core.Movement.CurrentVelocity.y < 0.0f)
        { 
            playerSm.JumpingState.DecreaseAmountOfJumpsLeft();
            
            playerSm.SwitchState(playerSm.FallingState);        
        }     
    }

    public override void Exit()
    {   
        base.Exit();

        playerSm.Core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeRotation); 
    }
}
