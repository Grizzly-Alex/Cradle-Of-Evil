using UnityEngine;

public sealed class PlayerCrouchingState : PlayerBaseState
{
    private readonly int HashIdleCrouch = Animator.StringToHash("IdleCrouch");
    private readonly int HashisMove = Animator.StringToHash("isMove"); 

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
        base.PhysicsUpdate();
                    
        core.Movement.MoveAlongSurface(playerData.CrouchingMoveSpeed * input.NormInputX);

        SwitchPhysMaterial(input.NormInputX);
    }

    public override void Exit()
    {
        input.SitStandEvent -= OnStand;    
    }

    private void OnStand()
    {
        if(!core.CollisionSenses.DetectingRoof())
        {
            stateMachine.SwitchState(new PlayerSitOrStandState(stateMachine, isTransitToCrouch: false)); 
        }      
    } 
}