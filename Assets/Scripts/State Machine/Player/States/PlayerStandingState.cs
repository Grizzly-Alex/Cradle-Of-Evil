using UnityEngine;

public sealed class PlayerStandingState : PlayerBaseState
{       
    private readonly int _hashIdleStand = Animator.StringToHash("IdleStand");
    private readonly int _hashRun = Animator.StringToHash("RunStart");
    private readonly int _hashisMove = Animator.StringToHash("isMove"); 

    public PlayerStandingState(PlayerStateMachine playerSm) : base(playerSm)
    {
    }

    public override void Enter()
    {
        base.Enter();
   
        SetColliderHeight(playerSm.Data.StandingColiderHeight);
    
        playerSm.Input.SitStandEvent += OnSit;
        playerSm.Input.JumpEvent += OnJump;
        playerSm.Input.DashEvent += OnDash;

        switch (playerSm.Input.NormInputX != 0)
        {
            case true : playerSm.Animator.Play(_hashRun); break; 
            case false: playerSm.Animator.Play(_hashIdleStand); break; 
        }
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
      
        playerSm.Core.Movement.MoveAlongSurface(playerSm.Data.StandingMoveSpeed * playerSm.Input.NormInputX); 
        playerSm.Core.Movement.SwitchRbConstraints(playerSm.Input.NormInputX);  
    }

    public override void Exit()
    {   
        base.Exit();

        playerSm.Input.SitStandEvent -= OnSit;  
        playerSm.Input.JumpEvent -= OnJump;
        playerSm.Input.DashEvent -= OnDash;

        playerSm.Core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeRotation); 
    }

    #region InputMethods
    private void OnJump() => playerSm.SwitchState(playerSm.JumpingState);

    private void OnSit()
    {
        playerSm.SwitchState(playerSm.SitDownState);   
    }

    private void OnDash()
    {
        playerSm.DashingState.PreviousState = PreviousState.Standing;
        playerSm.SwitchState(playerSm.DashingState);
    }        
    #endregion
}