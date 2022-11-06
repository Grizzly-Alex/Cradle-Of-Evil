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
        input.SitStandEvent += OnStand;

        animator.Play(HashIdleCrouch);  
    }

    public override void FrameUpdate()
    {
        stateMachine.Core.Movement.CheckIfShouldFlip(input.NormInputX);

        animator.SetBool(HashisMove, input.NormInputX != 0 ? true : false);
    
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
    }

    private void OnStand() => stateMachine.SwitchState(new PlayerSitOrStandState(stateMachine, isTransitToCrouch: false));
}