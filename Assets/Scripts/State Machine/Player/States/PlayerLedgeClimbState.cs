using UnityEngine;

public sealed class PlayerLedgeClimbState: PlayerBaseState
{
    public Vector2 DetectedPos {get; set;}
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 cornerPos;
    private Vector2 workspace;
    private bool isHanging;
    private bool isClimbing;
    private bool isTouchingCeiling;
    private readonly float defaultContactOffset = Physics2D.defaultContactOffset;
    private readonly int HashLedgeGrab = Animator.StringToHash("LedgeGrab");
    private readonly int isClimbingHash = Animator.StringToHash("isClimbing");


    public PlayerLedgeClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.JumpingState.ResetAmountOfJumpsLeft();

        core.Movement.SetVelocityZero();
        stateMachine.transform.position = DetectedPos;

        cornerPos = GetCornerOfLedge();

        startPos.Set(
            cornerPos.x - (stateMachine.BodyCollider.size.x / 2 + defaultContactOffset) * core.Movement.FacingDirection,
            cornerPos.y + Mathf.Abs(stateMachine.BodyCollider.offset.y) - stateMachine.BodyCollider.size.y /2);

        stopPos.Set(
            cornerPos.x + stateMachine.BodyCollider.size.x * core.Movement.FacingDirection,
            cornerPos.y + Mathf.Abs(stateMachine.BodyCollider.offset.y) + stateMachine.BodyCollider.size.y /2 + defaultContactOffset);

        stateMachine.transform.position = startPos;   
 
        animator.Play(HashLedgeGrab);
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isTouchingCeiling = CheckForSpace();

        Debug.Log(isTouchingCeiling);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if(isTouchingCeiling)
            {
                stateMachine.SwitchState(stateMachine.CrouchingState);
            }
            else
            {
                stateMachine.SitOrStandState.TransitionTo = SitOrStandTransition.Standing;
                stateMachine.SwitchState(stateMachine.SitOrStandState);
            }

        }
        else
        {
            core.Movement.SetVelocityZero();
            stateMachine.transform.position = startPos;

            if (core.Movement.FacingDirection == input.NormInputX && isHanging && !isClimbing)
            {
                isClimbing = true;
                animator.SetBool(isClimbingHash, true);
            }
            else if (input.NormInputY == -1 && isHanging && !isClimbing)
            {
                stateMachine.FallingState.ResetGrabLedge();
                stateMachine.SwitchState(stateMachine.FallingState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;
        animator.SetBool(isClimbingHash, false);

        if (isTouchingCeiling)
        {
            SetColliderHeight(playerData.CrouchingColiderHeight);
        }

        if (isClimbing)
        {
            stateMachine.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void AnimationTrigger() => isHanging = true;

    private bool CheckForSpace()
    {
        return Physics2D.Raycast(
            cornerPos + (Vector2.up * defaultContactOffset) + (Vector2.right * core.Movement.FacingDirection * defaultContactOffset),
            Vector2.up,
            playerData.StandingColiderHeight,
            core.CollisionSensors.PlatformsLayer);
    }

    private Vector2 GetCornerOfLedge()
    {
        float positionX = core.Movement.FacingDirection * core.CollisionSensors.WallHit.distance + core.CollisionSensors.WallSensor.position.x;

        RaycastHit2D hitDown = Physics2D.Raycast(
            new Vector2(positionX, core.CollisionSensors.LedgeHorizontalSensor.position.y),
            Vector2.down,
            core.CollisionSensors.LedgeHorizontalSensor.position.y - core.CollisionSensors.WallSensor.position.y,
            core.CollisionSensors.PlatformsLayer);

        float positionY = core.CollisionSensors.LedgeHorizontalSensor.position.y - hitDown.distance;

        return new Vector2(positionX, positionY);
    }    
}