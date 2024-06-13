using CoreSystem.CoreComponents.CollisionManipulationComponents;


namespace CoreSystem.CoreComponents
{
    public sealed class CollisionManipulation : CoreComponent
    {
        public PlatformCollision PlatformCollision { get; private set; }
        public BodyCollision BodyCollision { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            PlatformCollision = GetComponent<PlatformCollision>();
            BodyCollision = GetComponent<BodyCollision>();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            PlatformCollision.LogicUpdate();
            BodyCollision.LogicUpdate();
        }
    }
}
