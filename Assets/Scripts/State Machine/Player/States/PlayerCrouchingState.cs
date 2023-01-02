using UnityEngine;

public sealed class PlayerCrouchingState : PlayerBaseState
{
    private readonly int _hashIdleCrouch = Animator.StringToHash("IdleCrouch");
    private readonly int _hashisMove = Animator.StringToHash("isMove"); 
    private bool isCellingDetected;

    public PlayerCrouchingState(PlayerStateMachine playerSM) : base(playerSM)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        SetColliderHeight(playerSm.Data.CrouchingColiderHeight);

        playerSm.Input.SitStandEvent += OnStand;
        playerSm.Input.JumpEvent += OnJump;
        playerSm.Input.DashEvent += OnDash;

        playerSm.Animator.Play(_hashIdleCrouch); 
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isCellingDetected = playerSm.Core.CollisionSensors.CellingDetect;        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        playerSm.Core.Movement.CheckIfShouldFlip(playerSm.Input.NormInputX);

        playerSm.Animator.SetBool(_hashisMove, playerSm.Input.NormInputX != 0 ? true : false);
    
        if(!_isGrounded && playerSm.Core.Movement.CurrentVelocity.y < 0.0f)
        {
            playerSm.JumpingState.DecreaseAmountOfJumpsLeft();
                        
            playerSm.SwitchState(playerSm.FallingState);        
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
                    
        playerSm.Core.Movement.MoveAlongSurface(playerSm.Data.CrouchingMoveSpeed * playerSm.Input.NormInputX);
        playerSm.Core.Movement.SwitchRbConstraints(playerSm.Input.NormInputX);
    }

    public override void Exit()
    {
        base.Exit();

        playerSm.Input.SitStandEvent -= OnStand;  
        playerSm.Input.JumpEvent -= OnJump;  
        playerSm.Input.DashEvent -= OnDash;

        playerSm.Core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeRotation); 
    }

    #region InputMethods
    private void OnStand()
    {
        if(!isCellingDetected)
        {   
            playerSm.SwitchState(playerSm.StandUpState); 
        }      
    } 

    private void OnJump()
    {
        if(!isCellingDetected)
        {          
            playerSm.SwitchState(playerSm.JumpingState);
        }
    } 

    private void OnDash()
    {
        playerSm.DashingState.PreviousState = PreviousState.Crouching;
        playerSm.SwitchState(playerSm.DashingState);
    }        
    #endregion
}