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

        CheckFallingForce(core.Movement.CurrentVelocity);

        if(isGrounded)
        {
            stateMachine.SwitchState(new PlayerLandingState(stateMachine, fallingForce));
        }     
    }
 
    private void CheckFallingForce(Vector2 velocity)    
    {
        if (fallingForce > velocity.y)
        {            
            fallingForce = velocity.y;
        }
    }
    
}