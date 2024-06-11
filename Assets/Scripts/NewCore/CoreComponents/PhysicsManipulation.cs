using NewCore.CoreComponents.PhysicsManipulationComponents;
using UnityEngine;


namespace NewCoreSystem.CoreComponents
{
    public sealed class PhysicsManipulation : CoreComponent
    {
        public Movement Movement { get; private set; }
        public Flipping Flipping { get; private set; }
        public Freezing Freezing { get; private set; }
        public Gravitation Gravitation { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            Movement = GetComponent<Movement>();
            Flipping = GetComponent<Flipping>();
            Freezing = GetComponent<Freezing>();
            Gravitation = GetComponent<Gravitation>();         
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Movement.LogicUpdate();       
            Flipping.LogicUpdate();
            Freezing.LogicUpdate();
            Gravitation.LogicUpdate();
        }
    }
}
