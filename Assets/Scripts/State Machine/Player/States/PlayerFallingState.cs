using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerInAirState
{
    private readonly int HashFallingState = Animator.StringToHash("FallingState");
    
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Falling");
        animator.Play(HashFallingState);       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGrounded)
        {
            stateMachine.SwitchState(new PlayerStandingState(stateMachine));
            // switch landing state
        }     
    }
}
