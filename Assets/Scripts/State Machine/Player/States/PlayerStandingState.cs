using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingState : PlayerBaseState
{    
    private readonly int HashIdleStand = Animator.StringToHash("IdleStand");
    private readonly int HashRunStart = Animator.StringToHash("RunStart");
    private readonly int HashRun = Animator.StringToHash("Run");
    private readonly int HashRunStop = Animator.StringToHash("RunStop");
    private readonly int HashWalck = Animator.StringToHash("Walck");
    

    private bool isGrounded;


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
        
        if(isGrounded)
        {
            //transition air state         
        }
    }

    public override void PhysicsUpdate()
    {
        isGrounded = core.CollisionSenses.DetectingGround();
               
        core.Movement.MoveAlongSurface(data.StandingMoveSpeed * input.NormInputX);
    }

    public override void Exit()
    {
        Debug.Log("Exit: Standing State");    
    }
}