using UnityEngine;

public sealed class PlayerStateMachine: StateMachine
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField] public CapsuleCollider2D BodyCollider { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public Core Core { get; private set; }
    [field: SerializeField] public PlayerData PlayerData { get; private set; }

    #region States of behaviours
    public PlayerStandingState StandingState { get; private set; }
    public PlayerCrouchingState CrouchingState { get; private set; }
    public PlayerSitOrStandState SitOrStandState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerLandingState LandingState { get; private set; }
    public PlayerDashingState DashingState { get; private set; }
    #endregion

    private void Awake()
    {
        SitOrStandState = new PlayerSitOrStandState(this);
        CrouchingState = new PlayerCrouchingState(this);
        StandingState = new PlayerStandingState(this);
        JumpingState = new PlayerJumpingState(this);
        FallingState = new PlayerFallingState(this);
        LandingState = new PlayerLandingState(this); 
        DashingState = new PlayerDashingState(this);  
        InAirState = new PlayerInAirState(this);  
    }

    private void Start()
    {
        InitState(StandingState);
    }   
}