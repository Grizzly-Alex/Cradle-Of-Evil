using UnityEngine;

public sealed class PlayerLedgeClimbState: PlayerBaseState
{
    public Vector2 DetectedPos {get; set;}
    private Vector2 _startPos;
    private Vector2 _stopPos;
    private Vector2 _cornerPos;
    private bool _isHanging;
    private bool _isClimbing;
    private bool _isTouchingCeiling;
    private readonly int _hashLedgeGrab = Animator.StringToHash("LedgeGrab");
    private readonly int _isClimbingHash = Animator.StringToHash("isClimbing");


    public PlayerLedgeClimbState(PlayerStateMachine playerSm) : base(playerSm)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerSm.JumpingState.ResetAmountOfJumpsLeft();

        playerSm.Core.Movement.SetVelocityZero();
        playerSm.transform.position = DetectedPos;

        _cornerPos = GetCornerOfLedge();

        _startPos.Set(
            _cornerPos.x - (playerSm.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * playerSm.Core.Movement.FacingDirection,
            _cornerPos.y + Mathf.Abs(playerSm.BodyCollider.offset.y) - playerSm.BodyCollider.size.y /2);

        _stopPos.Set(
            _cornerPos.x + playerSm.BodyCollider.size.x * playerSm.Core.Movement.FacingDirection,
            _cornerPos.y + Mathf.Abs(playerSm.BodyCollider.offset.y) + playerSm.BodyCollider.size.y /2 + Physics2D.defaultContactOffset);

        playerSm.transform.position = _startPos;   
 
        playerSm.Animator.Play(_hashLedgeGrab);
    }

    public override void DoCheck()
    {
        base.DoCheck();

        _isTouchingCeiling = CheckForSpace();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAnimationFinished)
        {
            if(_isTouchingCeiling)
            {
                playerSm.SwitchState(playerSm.CrouchingState);
            }
            else
            {
                playerSm.SwitchState(playerSm.StandUpState);
            }
        }
        else
        {
            playerSm.Core.Movement.SetVelocityZero();
            playerSm.transform.position = _startPos;

            if (playerSm.Core.Movement.FacingDirection == playerSm.Input.NormInputX && _isHanging && !_isClimbing)
            {
                _isClimbing = true;
                playerSm.Animator.SetBool(_isClimbingHash, true);
            }
            else if (playerSm.Input.NormInputY == -1 && _isHanging && !_isClimbing)
            {
                playerSm.FallingState.ResetGrabLedge();
                playerSm.SwitchState(playerSm.FallingState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;
        playerSm.Animator.SetBool(_isClimbingHash, false);

        if (_isTouchingCeiling)
        {
            SetColliderHeight(playerSm.Data.CrouchingColiderHeight);
        }

        if (_isClimbing)
        {
            playerSm.transform.position = _stopPos;
            _isClimbing = false;
        }
    }

    public override void AnimationTrigger() => _isHanging = true;

    private bool CheckForSpace()
    {
        return Physics2D.Raycast(
            _cornerPos + (Vector2.up * Physics2D.defaultContactOffset) + (Vector2.right * playerSm.Core.Movement.FacingDirection * Physics2D.defaultContactOffset),
            Vector2.up,
            playerSm.Data.StandingColiderHeight,
            playerSm.Core.CollisionSensors.PlatformsLayer);
    }

    private Vector2 GetCornerOfLedge()
    {
        float positionX = playerSm.Core.Movement.FacingDirection * playerSm.Core.CollisionSensors.WallHit.distance + playerSm.Core.CollisionSensors.WallSensor.position.x;

        RaycastHit2D hitDown = Physics2D.Raycast(
            new Vector2(positionX, playerSm.Core.CollisionSensors.LedgeHorizontalSensor.position.y),
            Vector2.down,
            playerSm.Core.CollisionSensors.LedgeHorizontalSensor.position.y - playerSm.Core.CollisionSensors.WallSensor.position.y,
            playerSm.Core.CollisionSensors.PlatformsLayer);

        float positionY = playerSm.Core.CollisionSensors.LedgeHorizontalSensor.position.y - hitDown.distance;

        return new Vector2(positionX, positionY);
    }    
}