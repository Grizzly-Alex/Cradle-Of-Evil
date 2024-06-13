using CoreSystem.CoreComponents.SensorDetectComponents;

namespace CoreSystem.CoreComponents
{
    public sealed class SensorDetect : CoreComponent
    {
        public GroundDetector GroundDetector { get; private set; }
        public GrabWallDetector GrabWallDetector { get; private set; }
        public LedgeDetector LedgeDetector { get; private set; }
        public CeilingDetector CeilingDetector { get; private set; }
        public GirderDetector GirderDetector { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            GroundDetector = GetComponent<GroundDetector>();
            GrabWallDetector = GetComponent<GrabWallDetector>();
            LedgeDetector = GetComponent<LedgeDetector>();
            CeilingDetector = GetComponent<CeilingDetector>();
            GirderDetector = GetComponent<GirderDetector>();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }
    }
}
