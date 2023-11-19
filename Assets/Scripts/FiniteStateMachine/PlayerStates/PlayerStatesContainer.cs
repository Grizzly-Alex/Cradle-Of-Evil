using Entities;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerStatesContainer
    {
        public PlayerBaseState PreviousState { get; set; }
        public PlayerInAirState InAir { get; private set; }
        public PlayerJumpState Jump { get; private set; }
        public PlayerLandingState Landing { get; private set; }
        public PlayerCrouchState Crouch { get; private set; }
        public PlayerStandState Stand { get; private set; }
        public PlayerSlideState Slide { get; private set; }
        public PlayerSitStandState SitStand { get; private set; }
        public PlayerLedgeClimbState LedgeClimb { get; private set; }
        public PlayerOnWallState OnWall { get; private set; }
        public PlayerDashState Dash{ get; private set; }

        public PlayerStatesContainer(StateMachine stateMachine, Player player)
        {
            InAir = new PlayerInAirState(stateMachine, player);
            Jump = new PlayerJumpState(stateMachine, player);
            Landing = new PlayerLandingState(stateMachine, player);
            Stand = new PlayerStandState(stateMachine, player);
            Crouch = new PlayerCrouchState(stateMachine, player);
            Slide = new PlayerSlideState(stateMachine, player);
            SitStand = new PlayerSitStandState(stateMachine, player);
            LedgeClimb = new PlayerLedgeClimbState(stateMachine, player);
            OnWall = new PlayerOnWallState(stateMachine, player);
            Dash = new PlayerDashState(stateMachine, player);
        }
    }
}
