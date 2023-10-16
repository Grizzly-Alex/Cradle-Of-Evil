using FiniteStateMachine.PlayerStates;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities
{
    public sealed class Player : Entity
    {
        #region States of behaviours
        public PlayerInAirState InAirState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerLandingState LandingState { get; private set; }
        public PlayerCrouchState CrouchState { get; private set; }
        public PlayerStandState StandState { get; private set;}
        public PlayerGroundDashState GroundDashState { get; private set; }  
        public PlayerSitStandState SitStandState {  get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerOnWallState OnWallState { get; private set; }
        public PlayerLevitationDashState LevitationDashState { get; private set; }
        #endregion

        [field: SerializeField]
        public PlayerData Data { get; private set; }
        public InputReader Input { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InAirState = new PlayerInAirState(stateMachine, this);
            JumpState = new PlayerJumpState(stateMachine, this);
            LandingState = new PlayerLandingState(stateMachine, this);
            StandState = new PlayerStandState(stateMachine, this);
            CrouchState = new PlayerCrouchState(stateMachine, this);
            GroundDashState = new PlayerGroundDashState(stateMachine, this);
            SitStandState = new PlayerSitStandState(stateMachine, this);
            LedgeClimbState = new PlayerLedgeClimbState(stateMachine, this);
            WallJumpState = new PlayerWallJumpState(stateMachine, this);
            OnWallState = new PlayerOnWallState(stateMachine, this);
            LevitationDashState = new PlayerLevitationDashState(stateMachine, this);
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