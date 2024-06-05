using NewCoreSystem;
using Pool;
using System;
using UnityEngine;

namespace NewCore.CoreComponents.VisualFxComponents
{
    [Serializable]
    public class ShadowFx : VisualFxComponent
    {
        [SerializeField]
        private bool shadowOn;
        [SerializeField]
        private float coefficientShadowScale;
        [SerializeField]
        private GameObject prefab;

        private Pool.ItemsPool.Shadow shadowFromPool;
        private Vector3 initialShadowScale;
        private Vector3 scaleChange;
        private SpriteRenderer entitySpriteRenderer;

        public ShadowFx(Core core) : base(core)
        {
            entitySpriteRenderer = core.GetComponentInParent<SpriteRenderer>();
        }

        public override void LogicUpdate()
        {
            if (shadowOn)
            {
                UpdateShadow();
            }
        }

        private Pool.ItemsPool.Shadow CreateShadow()
        {
            Pool.ItemsPool.Shadow shadow = PoolManager.Instance.GetFromPool<Pool.ItemsPool.Shadow>(prefab, entityTransform.position, entityTransform.rotation);

            if (shadow != null)
            {
                shadow.Initialize(entitySpriteRenderer);
            }
            return shadow;
        }

        private void UpdateShadow()
        {
            Vector2 position = core.Sensor.GroundDetector.GroundHit.point;
            bool isActive = shadowFromPool.isActiveAndEnabled;

            if (isActive) shadowFromPool.transform.SetPositionAndRotation(position, entityTransform.rotation);

            if (core.Sensor.GroundDetector.IsGroundDetect())
            {
                if (shadowFromPool.transform.localScale == initialShadowScale) return;
                shadowFromPool.transform.localScale = initialShadowScale;
            }
            else
            {
                float distance = Vector3.Distance(entityTransform.position, position);
                float calculateCoefficient = distance / coefficientShadowScale;

                scaleChange = new Vector3(
                        initialShadowScale.x - calculateCoefficient * initialShadowScale.x,
                        initialShadowScale.y - calculateCoefficient * initialShadowScale.y);

                if (float.IsNegative(scaleChange.y) || float.IsNegative(scaleChange.x))
                {
                    if (!isActive) return;
                    shadowFromPool.ReturnToPool();
                }
                else
                {
                    if (!isActive) CreateShadow();
                    shadowFromPool.transform.localScale = scaleChange;
                }
            }
        }

        public void ShadowIsActive(bool isActive)
        {
            shadowOn = isActive;

            if (!isActive && shadowFromPool.isActiveAndEnabled)
            {
                shadowFromPool.ReturnToPool();
            }

            if (isActive && !shadowFromPool.isActiveAndEnabled)
            {
                CreateShadow();
            }
        }
    }
}
