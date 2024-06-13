using CoreSystem.CoreComponents.VisualFxComponents;


namespace CoreSystem.CoreComponents
{
    public class VisualFx : CoreComponent
    {
        public ShadowFx Shadow  { get;  private set; }
        public AfterImageFx AfterImage { get; private set; }
        public AnimationFx AnimationFx { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            Shadow = GetComponent<ShadowFx>();
            AfterImage = GetComponent<AfterImageFx>();
            AnimationFx = GetComponent<AnimationFx>();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Shadow.LogicUpdate();
            AfterImage.LogicUpdate();
            AnimationFx.LogicUpdate();
        }
    }
}