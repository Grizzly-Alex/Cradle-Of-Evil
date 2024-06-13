using NewCore.CoreComponents.SensorDetectComponents;
using UnityEngine;

namespace Assets.Scripts.NewCore.CoreComponents.SensorDetectComponents
{
    public class GirderDetector : SensorDetectComponent
    {
        [SerializeField] private float circleRadius;
        [SerializeField] private float positionOffsetY;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Grid grid;

        protected override string SensorName => nameof(GirderDetector);

        protected override Vector2 InitSensorPosition => entityCollider.bounds.center;



        public Collider2D GirderCollider => Physics2D.OverlapCircle(
            new Vector2 (sensor.position.x, sensor.position.y + positionOffsetY),
            circleRadius,
            targetLayer);


        public bool IsGirderDetect() => GirderCollider;

        public bool GetDetectedGirderPosition(out Vector2 girderPosition)
        {
            girderPosition = Vector2.zero;
            bool isDetected = IsGirderDetect();

            if (isDetected)
            {
                Vector3 detectedPoint = GirderCollider.ClosestPoint(sensor.position);
                Vector3Int cellPosition = grid.WorldToCell(detectedPoint);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
                girderPosition.Set(centerOfCell.x, centerOfCell.y);
            }
            return isDetected;
        }


        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(sensor.position.x, sensor.position.y + positionOffsetY), circleRadius); //girder ray 
        }
    }
}
