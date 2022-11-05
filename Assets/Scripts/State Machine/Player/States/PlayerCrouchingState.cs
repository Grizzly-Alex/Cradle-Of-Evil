using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchingState : PlayerBaseState
{
    private readonly int HashIdleCrouch = Animator.StringToHash("IdleCrouch");
    private readonly int HashisMove = Animator.StringToHash("isMove"); 
    private bool isGrounded;


    public PlayerCrouchingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter: Crouching State");

        input.SitStandEvent += OnStand;

        stateMachine.Animator.Play(HashIdleCrouch);      
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
               
        core.Movement.MoveAlongSurface(data.CrouchingMoveSpeed * input.NormInputX);
    }

    public override void Exit()
    {
        input.SitStandEvent -= OnStand;
        
        Debug.Log("Exit: Crouching State");    
    }

    private void OnStand() => stateMachine.SwitchState(new PlayerStandingState(stateMachine));
}