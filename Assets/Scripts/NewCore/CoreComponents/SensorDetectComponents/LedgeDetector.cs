using NewCore.CoreComponents.SensorDetectComponents;
using UnityEngine;

namespace Assets.Scripts.NewCore.CoreComponents.SensorDetectComponents
{
    public class LedgeDetector : SensorDetectComponent
    {
        private Vector2 sensorPosition;

        [SerializeField] private float hitDistance;
        [SerializeField] private float positionOffsetY;
        [SerializeField] private float spanOfLedge;
        [SerializeField] public LayerMask targetLayer;
        [SerializeField] public Grid grid;

        protected override string SensorName => nameof(LedgeDetector);

        protected override Vector2 InitSensorPosition => default;



        private RaycastHit2D LedgeHit => Physics2D.Raycast(
            sensorPosition,
            Vector2.right * core.Physics.Flipping.FacingDirection,
            hitDistance,
            targetLayer);


        public bool IsLedgeDetect()
        {
            bool aboveIsEmpty = !Physics2D.Raycast(
                new Vector2(sensorPosition.x, sensorPosition.y + spanOfLedge),
                Vector2.right * core.Physics.Flipping.FacingDirection,
                hitDistance,
                targetLayer);

            bool betweenHitsIsEmpty = !Physics2D.Raycast(
                sensorPosition,
                Vector2.up,
                spanOfLedge,
                targetLayer);

            return LedgeHit.collider != null
                && betweenHitsIsEmpty
                && aboveIsEmpty;
        }


        public bool GetDetectedLedgeCorner(out Vector2 ledgeCorner)
        {
            ledgeCorner = Vector2.zero;
            bool isDetected = IsLedgeDetect();

            if (isDetected)
            {
                Vector3Int cellPosition = grid.WorldToCell(LedgeHit.point);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
                ledgeCorner.Set(centerOfCell.x, centerOfCell.y);
            }

            return isDetected;
        }

        private void OnDrawGizmos()
        { 
            if (!Application.isPlaying) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(new Vector2(sensorPosition.x, sensorPosition.y + spanOfLedge), new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0)); //ledge ray 1
            Gizmos.DrawRay(sensorPosition, new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0)); //ledge ray 2
        }
    }
}
