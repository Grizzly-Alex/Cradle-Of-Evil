using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingState : PlayerBaseState
{    
    private readonly int HashIdleStand = Animator.StringToHash("IdleStand");
    private readonly int HashRunStart = Animator.StringToHash("RunStart");
    private readonly int HashRunStop = Animator.StringToHash("RunStop");
    private readonly int HashWalck = Animator.StringToHash("RunStop");
    private readonly int HashRun = Animator.StringToHash("Run");

    public Vector2 movement;

    public PlayerStandingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter: Standing State");
        
    }
    public override void FrameUpdate()
    {
        stateMachine.Core.Movement.CheckIfShouldFlip(input.NormInputX);
        core.CollisionSenses.DetectingGround();

    }

    public override void PhysicsUpdate()
    {
        core.Movement.SetVelocityX(data.StandingMoveSpeed * input.NormInputX);  
    }

    public override void Exit()
    {
        Debug.Log("Exit: Standing State");    
    }
}