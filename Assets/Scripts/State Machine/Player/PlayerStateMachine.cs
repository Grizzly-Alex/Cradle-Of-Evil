using UnityEngine;

public sealed class PlayerStateMachine: StateMachine
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField] public CapsuleCollider2D BodyCollider { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public InputReader Input { get; private set; }
    [field: SerializeField] public Core Core { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }

    #region States of behaviours
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerTransitionState SitDownState { get; private set; }
    public PlayerTransitionState StandUpState { get; private set; }
    public PlayerCrouchingState CrouchingState { get; private set; }
    public PlayerStandingState StandingState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerLandingState LandingState { get; private set; }
    public PlayerDashingState DashingState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerGrabWallState GrabWallState { get; private set; }
    #endregion

    private void Awake()
    {
        LedgeClimbState = new PlayerLedgeClimbState(this);
        CrouchingState = new PlayerCrouchingState(this);
        StandingState = new PlayerStandingState(this);
        JumpingState = new PlayerJumpingState(this);
        FallingState = new PlayerFallingState(this);
        LandingState = new PlayerLandingState(this); 
        DashingState = new PlayerDashingState(this);  
        InAirState = new PlayerInAirState(this);
        GrabWallState = new PlayerGrabWallState(this);
        SitDownState = new PlayerTransitionState(this, "SitDown", CrouchingState); 
        StandUpState = new PlayerTransitionState(this, "StandUp", StandingState); 
    }

    private void Start()
    {
        InitState(StandingState);
    }   
}