using UnityEngine;

public class PlayerOnWallState : PlayerBaseState
{
    public Vector2 DetectedPointWall {get; set;}
    private Vector2 _holdPosition;
    private readonly int _landingOnWall = Animator.StringToHash("LandingOnWall");
    
    public PlayerOnWallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerSm.Core.Movement.SetVelocityZero();
        _holdPosition.Set(DetectedPointWall.x - (playerSm.BodyCollider.size.x /2 + Physics2D.defaultContactOffset) * playerSm.Core.Movement.FacingDirection,
        DetectedPointWall.y);

        playerSm.transform.position = _holdPosition;

        playerSm.Animator.Play(_landingOnWall);
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        playerSm.transform.position = _holdPosition;  

        if (_isAnimationFinished)
        {
            //TODO
        }     
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }


}
