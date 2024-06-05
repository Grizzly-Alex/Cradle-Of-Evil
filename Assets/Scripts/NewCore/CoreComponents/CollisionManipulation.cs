using NewCore.CoreComponents.CollisionManipulationComponents;
using UnityEngine;


namespace NewCoreSystem.CoreComponents
{
    public sealed class CollisionManipulation : CoreComponent
    {
        [field: SerializeField]
        public PlatformCollision PlatformCollision { get; private set; }

        [field: SerializeField]
        public BodyCollision BodyCollision { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlatformCollision = new(core);
            BodyCollision = new(core);
        }
    }
}
