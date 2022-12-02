using UnityEngine;

public sealed class PlayerLedgeClimbState : PlayerBaseState
{
    public Vector2 DetectedPos {get; set;}
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 corner;
    private Vector2 workspace;
    private readonly float defaultContactOffset = Physics2D.defaultContactOffset;

    public PlayerLedgeClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        stateMachine.transform.position = DetectedPos;

        corner = GetCornerOfLedge();
        stateMachine.transform.position = corner;
        Debug.Log($"Corner: {corner}");

        Debug.Log($"hit dictance: {core.CollisionSensors.LedgeHorizontalSensor.position.y - core.CollisionSensors.WallSensor.position.y}");

        
  
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.SetVelocityZero();
        stateMachine.transform.position = corner;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
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