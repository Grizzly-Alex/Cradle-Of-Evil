namespace CoreSystem.CoreComponents.PhysicsManipulationComponents
{
    public class Gravitation : PhysicsManipulationComponent
    {
        private float defaultGravityScale;

        protected override void Start()
        {
            base.Start();

            defaultGravityScale = body.gravityScale;
        }

        public void GravitationOn() => body.gravityScale = defaultGravityScale;
        public void GravitationOff() => body.gravityScale = 0.0f;
    }
}
