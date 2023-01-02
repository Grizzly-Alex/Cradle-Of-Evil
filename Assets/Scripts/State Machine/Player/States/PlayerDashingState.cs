using UnityEngine;

public enum PreviousState { Standing, Crouching } 

public sealed class PlayerDashingState : PlayerBaseState
{
    public PreviousState PreviousState { get; set; }
    private float _startTime;
    private bool _timeOut;
    private bool _isCellingDetected;
    private readonly int _hashStartDash = Animator.StringToHash("StartDash");
    private readonly int _hashDashing = Animator.StringToHash("Dashing");
    private readonly int _hashToStand = Animator.StringToHash("toStand");
    private readonly int _hashToCrouch = Animator.StringToHash("toCrouch");

    public PlayerDashingState(PlayerStateMachine playerSm) : base(playerSm)
    {
    }

    public override void Enter()
    {
        base.Enter();
 
        _timeOut = false;
        _startTime = Time.time;

        SetColliderHeight(playerSm.Data.CrouchingColiderHeight);

        switch (PreviousState)
        {
            case PreviousState.Standing: playerSm.Animator.Play(_hashStartDash); break;
            case PreviousState.Crouching: playerSm.Animator.Play(_hashDashing); break;
        }
    }

    public override void DoCheck()
    {
        base.DoCheck();

        _isCellingDetected = playerSm.Core.CollisionSensors.CellingDetect;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!_isGrounded)
        {
            playerSm.SwitchState(playerSm.FallingState);
        }

        if (Time.fixedTime >= _startTime + playerSm.Data.DashingTime)
        {         
            _timeOut = true; 

            if (_isCellingDetected)
            {
                playerSm.Animator.SetTrigger(_hashToCrouch);
               
                if (_isAnimationFinished)
                {
                    playerSm.SwitchState(playerSm.CrouchingState);
                }                   
            }
            else 
            {             
                switch (PreviousState)
                {
                    case PreviousState.Standing: playerSm.Animator.SetTrigger(_hashToStand); break;
                    case PreviousState.Crouching: playerSm.Animator.SetTrigger(_hashToCrouch); break;
                }                 
                
                if (_isAnimationFinished)
                {
                    switch (PreviousState)
                    {
                        case PreviousState.Standing:
                            playerSm.SwitchState(playerSm.StandingState);
                            break;
                        case PreviousState.Crouching: 
                            playerSm.SwitchState(playerSm.CrouchingState); 
                            break;
                    }               
                }                
            }             
        }           
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!_timeOut)
        {
            playerSm.Core.Movement.MoveAlongSurface(playerSm.Data.DashingSpeed * playerSm.Core.Movement.FacingDirection);
        }
        else
        {
            playerSm.Core.Movement.SetVelocityZero();
        }
    }

    public override void Exit()
    {
        base.Exit(); 

        playerSm.Animator.ResetTrigger(_hashToStand);
        playerSm.Animator.ResetTrigger(_hashToCrouch);

        playerSm.Input.DashInputCooldown = playerSm.Data.DashingCooldown;          
    }
}