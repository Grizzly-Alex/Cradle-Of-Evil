using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingState : PlayerBaseState
{ 
       
    private readonly int HashIdleStand = Animator.StringToHash("IdleStand");
    private readonly int HashisMove = Animator.StringToHash("isMove");
    private bool isGrounded;


    public PlayerStandingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter: Standing State");
        
        input.SitStandEvent += OnSit;

        stateMachine.Animator.Play(HashIdleStand);
    }

    public override void FrameUpdate()
    {
        stateMachine.Core.Movement.CheckIfShouldFlip(input.NormInputX);

        stateMachine.Animator.SetBool(HashisMove, input.NormInputX != 0 ? true : false);

      
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
        input.SitStandEvent -= OnSit;

        Debug.Log("Exit: Standing State");    
    }

    private void OnSit() => stateMachine.SwitchState(new PlayerCrouchingState(stateMachine));
}