using NewCore.CoreComponents.SensorDetectComponents;
using UnityEngine;

namespace Assets.Scripts.NewCore.CoreComponents.SensorDetectComponents
{
    
    public class GrabWallDetector : SensorDetectComponent
    {
        private Vector2 sensorPositionUp;
        private Vector2 sensorPositionDown;

        [SerializeField] private float upOffsetPositionY;
        [SerializeField] private float downOffsetPositionY;
        [SerializeField] private float hitDistance;
        [SerializeField] public string targetTag;
        [SerializeField] public LayerMask targetLayer;

        protected override string SensorName => nameof(GrabWallDetector);

        protected override Vector2 InitSensorPosition => default;



        public RaycastHit2D WallHitUp => Physics2D.Raycast(
            sensorPositionUp,
            Vector2.right * core.Physics.Flipping.FacingDirection,
            hitDistance,
            targetLayer);

        public RaycastHit2D WallHitDown => Physics2D.Raycast(
            sensorPositionDown,
            Vector2.right * core.Physics.Flipping.FacingDirection,
            hitDistance,
            targetLayer);


        public bool IsGrabWallDetect()
        {
            if (WallHitUp.collider is null || WallHitDown.collider is null) return false;

            return WallHitUp.collider.CompareTag(targetTag) && WallHitDown.collider.CompareTag(targetTag);
        }

        public bool GetDetectedGrabWallPosition(out Vector2 wallPosition)
        {
            wallPosition = Vector2.zero;
            bool isDetected = IsGrabWallDetect();

            if (isDetected)
            {
                wallPosition = WallHitUp.point;
            }

            return isDetected;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;

            Gizmos.DrawRay(sensorPositionUp, new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0));
            Gizmos.DrawRay(sensorPositionDown, new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0));
        }
    }
}
