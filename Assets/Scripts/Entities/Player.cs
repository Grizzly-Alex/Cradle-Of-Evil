using FiniteStateMachine.PlayerStates;
using UnityEngine;

namespace Entities
{
    public sealed class Player : Entity
    {
        #region Data
        [field: Header("MOVEMENT")]
        [field: SerializeField] public float StandMoveSpeed { get; private set; }
        [field: SerializeField] public float CrouchMoveSpeed { get; private set; }
        [field: SerializeField] public float InAirMoveSpeed { get; private set; }

        [field: Header("JUMPING")]
        [field: SerializeField] public float FirstJumpForce { get; private set; }
        [field: SerializeField] public float SecondJumpForce { get; private set; }
        [field: SerializeField] public float WallJumpTime { get; private set; }
        public int AmountOfJump => 2;

        [field: Header("GRAB LEDGE")]
        [field: SerializeField] public float GrabLedgeCooldown { get; private set; }

        [field: Header("LANDING")]
        [field: SerializeField] public float LandingThreshold { get; private set; }

        [field: Header("DASHING")]
        [field: SerializeField] public float GroundDashSpeed { get; private set; }
        [field: SerializeField] public float GroundDashTime { get; private set; }
        [field: SerializeField] public float GroundDashCooldown { get; private set; }
        [field: SerializeField] public float AirDashSpeed { get; private set; }
        [field: SerializeField] public float AirDashTime { get; private set; }

        [field: Header("COLIDER SIZE")]
        [field: SerializeField] public float StandColiderHeight { get; private set; }
        [field: SerializeField] public float CrouchColiderHeight { get; private set; }
        #endregion

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
        #endregion

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