using NewCore.CoreComponents.SensorDetectComponents;
using NewCoreSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.NewCore.CoreComponents.SensorDetectComponents
{
    [Serializable]
    public class GirderDetector : SensorDetectComponent
    {
        private readonly Vector2 sensorPosition;

        [SerializeField] private float circleRadius;
        [SerializeField] private float positionOffsetY;
        [SerializeField] public LayerMask targetLayer;
        [SerializeField] private Grid grid;


        public GirderDetector(Core core) : base(core)
        {
            sensorPosition = new Vector2(entityCollider.bounds.max.y - positionOffsetY, entityCollider.bounds.center.x);
        }

        public Collider2D GirderCollider => Physics2D.OverlapCircle(
            sensorPosition,
            circleRadius,
            targetLayer);

        public bool IsGirderDetect() => GirderCollider;

        public bool GetDetectedGirderPosition(out Vector2 girderPosition)
        {
            girderPosition = Vector2.zero;
            bool isDetected = IsGirderDetect();

            if (isDetected)
            {
                Vector3 detectedPoint = GirderCollider.ClosestPoint(sensorPosition);
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
            Gizmos.DrawWireSphere(sensorPosition, circleRadius); //girder ray 
        }
    }
}
