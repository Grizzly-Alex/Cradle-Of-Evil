using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingState : PlayerBaseState
{
    public PlayerStandingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter: Statnding State");

    }
    public override void LogicUpdate(float deltaTime)
    {
   
    }

    public override void Exit()
    {
        Debug.Log("Exit: Statnding State");    
    }
}