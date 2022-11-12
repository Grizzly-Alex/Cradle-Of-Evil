using System;
using UnityEngine;

public class PlayerFallingState : PlayerInAirState
{
    private readonly int HashFallingState = Animator.StringToHash("FallingState");
    private float fallingForce;
    
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        animator.Play(HashFallingState);       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        SetFallingForce(core.Movement.CurrentVelocity);

        if(isGrounded)
        {
            stateMachine.LandingState.LandingForce = fallingForce;
            stateMachine.SwitchState(stateMachine.LandingState);
        }     
    }

    public override void Exit()
    {
        base.Exit();
        
        ResetFallingForce();
    }
 
    private void SetFallingForce(Vector2 velocity)    
    {
        if (fallingForce > velocity.y)
        {            
            fallingForce = velocity.y;
        }
    }

    private void ResetFallingForce() => fallingForce = 0.0f;
}