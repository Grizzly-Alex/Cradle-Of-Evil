using NewCore.CoreComponents.VisualFxComponents;
using UnityEngine;

namespace NewCoreSystem.CoreComponents
{
    public class VisualFx : CoreComponent
    {
        [field: SerializeField]
        public ShadowFx Shadow  { get; private set; }

        [field: SerializeField]
        public AfterImageFx AfterImage { get; private set; }

        [field: SerializeField]
        public AnimationFx AnimationFx { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            Shadow = new(core);
            AfterImage = new(core);
            AnimationFx = new(core);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Shadow.LogicUpdate();
        }
    }
}