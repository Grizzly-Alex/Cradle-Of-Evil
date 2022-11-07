using UnityEngine;

public sealed class PlayerCrouchingState : PlayerBaseState
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
               
        core.Movement.MoveAlongSurface(playerData.CrouchingMoveSpeed * input.NormInputX);

        Debug.Log(core.CollisionSenses.DetectingRoof());

        switch (input.NormInputX)
        {
            case 0: SetPhysicsMaterial(materialsData.Friction); break;
            default: SetPhysicsMaterial(materialsData.NoFriction); break;
        }
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