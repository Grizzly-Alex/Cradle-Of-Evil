using Assets.Scripts.NewCore.CoreComponents.SensorDetectComponents;
using NewCore.CoreComponents.PhysicsComponents;
using UnityEngine;

namespace NewCoreSystem.CoreComponents
{
    public sealed class SensorDetect : CoreComponent
    {
        [field: SerializeField]
        public GroundDetector GroundDetector { get; private set; }

        [field: SerializeField]
        public GrabWallDetector GrabWallDetector { get; private set; }

        [field: SerializeField]
        public LedgeDetector LedgeDetector { get; private set; }

        [field: SerializeField]
        public CeilingDetector CeilingDetector { get; private set; }

        [field: SerializeField]
        public GirderDetector GirderDetector { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            GroundDetector = new(core);
            GrabWallDetector = new(core);
            LedgeDetector = new(core);
            CeilingDetector = new(core);
            GirderDetector = new(core);
        }
    }
}
