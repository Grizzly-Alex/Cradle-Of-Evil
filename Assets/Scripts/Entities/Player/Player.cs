using FiniteStateMachine.PlayerStates;
using UnityEngine;


namespace Entities
{
    public sealed class Player : Entity
    {
        [field: SerializeField]
        public PlayerData Data { get; private set; }
        public InputReader Input { get; private set; }

        #region States
        public PlayerState PreviousState { get; set; }
        public PlayerOnGroundState LastOnGroundState { get; set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerLandingState LandingState { get; private set; }
        public PlayerCrouchState CrouchState { get; private set; }
        public PlayerStandState StandState { get; private set; }
        public PlayerSlideState SlideState { get; private set; }
        public PlayerSitStandState SitStandState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerOnWallState OnWallState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            InAirState = new PlayerInAirState(stateMachine, this);
            JumpState = new PlayerJumpState(stateMachine, this);
            LandingState = new PlayerLandingState(stateMachine, this);
            StandState = new PlayerStandState(stateMachine, this);
            CrouchState = new PlayerCrouchState(stateMachine, this);
            SlideState = new PlayerSlideState(stateMachine, this);
            SitStandState = new PlayerSitStandState(stateMachine, this);
            LedgeClimbState = new PlayerLedgeClimbState(stateMachine, this);
            OnWallState = new PlayerOnWallState(stateMachine, this);
            DashState = new PlayerDashState(stateMachine, this);
        }

        protected override void Start()
        {
            base.Start();

            Input = GetComponent<InputReader>();
            stateMachine.InitState(StandState);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}