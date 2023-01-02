using UnityEngine;

public sealed class PlayerFallingState: PlayerInAirState
{
    private bool _canGrabLedge = true;
    private float _fallingForce;
    private float _startTime;
    private bool _isLedgeDetected;
    private bool _isWallDetected;
    private readonly int _hashFallingState = Animator.StringToHash("FallingState");
    
    public PlayerFallingState(PlayerStateMachine playerSm) : base(playerSm)
    {       
    }

    public override void Enter()
    {
        base.Enter();

        _startTime = Time.time;

        playerSm.Animator.Play(_hashFallingState);                 
    }

    public override void DoCheck()
    {
        base.DoCheck();

        _isLedgeDetected = playerSm.Core.CollisionSensors.ledgeHorizontalDetect;
        _isWallDetected = playerSm.Core.CollisionSensors.GrabWallDetect;
      
        if (_isLedgeDetected)
        {
            playerSm.LedgeClimbState.DetectedPos = playerSm.transform.position;
        }
        else if (_isWallDetected)
        {
            playerSm.GrabWallState.DetectedPointWall = playerSm.Core.CollisionSensors.WallHit.point;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();  
    
        SetFallingForce(playerSm.Core.Movement.CurrentVelocity);

        if (_isGrounded)
        {
            playerSm.LandingState.LandingForce = _fallingForce;
            playerSm.SwitchState(playerSm.LandingState);
        }   
        else if (_isWallDetected)
        {
            playerSm.SwitchState(playerSm.GrabWallState);
        }  
        else if (_isLedgeDetected && CheckCanGrabLedge())
        {            
            playerSm.SwitchState(playerSm.LedgeClimbState);
        }      
    }

    public override void Exit()
    {
        base.Exit();
        
        ResetFallingForce();
        _canGrabLedge = true;       
    }

    public void ResetGrabLedge() => _canGrabLedge = false;

    private bool CheckCanGrabLedge() => !_canGrabLedge ? _canGrabLedge = Cooldown(playerSm.Data.GrabLedgeCooldown) : true;

    private bool Cooldown(float finishTime) => Time.time >= finishTime + _startTime;

    private void ResetFallingForce() => _fallingForce = 0.0f;

    private void SetFallingForce(Vector2 velocity)    
    {
        if (_fallingForce > velocity.y)
        {            
            _fallingForce = velocity.y;
        }
    }
}