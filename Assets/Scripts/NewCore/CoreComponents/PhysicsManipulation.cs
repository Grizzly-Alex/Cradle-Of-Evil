using NewCore.CoreComponents.PhysicsManipulationComponents;
using UnityEngine;


namespace NewCoreSystem.CoreComponents
{
    public sealed class PhysicsManipulation : CoreComponent
    {
        [field: SerializeField]
        public Movement Movement { get; private set; }

        [field: SerializeField]
        public Flipping Flipping { get; private set; }

        [field: SerializeField]
        public Freezing Freezing { get; private set; }

        [field: SerializeField]
        public Gravitation Gravitation { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            Movement = new(core);
            Flipping = new(core);
            Freezing = new(core);
            Gravitation = new(core);           
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Movement.LogicUpdate();         
        }
    }
}
