using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerSitOrStandState : PlayerBaseState
{
    private readonly int HashSitDown = Animator.StringToHash("SitDown");
    private readonly int HashStandUp = Animator.StringToHash("StandUp");
    private bool isGrounded;
    private bool isTransitToCrouch;


    public PlayerSitOrStandState(PlayerStateMachine stateMachine, bool isTransitToCrouch) : base(stateMachine)
    {
        this.isTransitToCrouch = isTransitToCrouch;
    }

    public override void Enter()
    {
        SetPhysicsMaterial(materialsData.Friction);
        
        if(isTransitToCrouch)
        {
            animator.Play(HashSitDown);
        }
        else
        {
            animator.Play(HashStandUp);
        }      
    }

    public override void FrameUpdate()
    {
        if(isAnimationFinished)
        {
            if(isTransitToCrouch)
            {
                SetColliderHeight(playerData.CrouchingColiderHeight);

                stateMachine.SwitchState(new PlayerCrouchingState(stateMachine));
            }
            else
            {
                SetColliderHeight(playerData.StandingColiderHeight);

                stateMachine.SwitchState(new PlayerStandingState(stateMachine));
            }            
        }

        if(isGrounded)
        {
            //transition air state         
        }  
    }

    public override void PhysicsUpdate()
    {
        isGrounded = core.CollisionSenses.DetectingGround();

        core.Movement.SetVelocityZero();
    }

    public override void Exit()
    {
   
    }
}
