using NewCoreSystem;
using System;


namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    [Serializable]
    public class Gravitation : PhysicsManipulationComponent
    {
        private readonly float defaultGravityScale;

        public Gravitation(Core core) : base(core)
        {
            defaultGravityScale = body.gravityScale;
        }

        public void GravitationOn() => body.gravityScale = defaultGravityScale;
        public void GravitationOff() => body.gravityScale = 0.0f;
    }
}
