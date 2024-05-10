using Pool;
using Pool.ItemsPool;
using System;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer entitySpriteRenderer;
        private Transform entityTransform;

        [Header("AFTER IMAGE")]
        [SerializeField] 
        private AfterImageSprite afterImagePrefab;
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        [SerializeField]
        private float colorLooseRate;
        private float lastImageXpos;

        [Header("ANIMATION FX")]
        [SerializeField]
        private Dust dustPrefab;
        [SerializeField]
        private AbilityFx abilityFXPrefab;

        [Header("SHADOW FX")]
        [SerializeField]
        private Shadow shadowPrefab;
        [SerializeField]
        private bool shadowOn;
        [SerializeField]
        private float coefficientShadowScale;
        private Shadow shadowFromPool;
        private Vector3 initialShadowScale;
        private Vector3 scaleChange;


        protected override void Awake()
        {
            base.Awake();
            entitySpriteRenderer = GetComponentInParent<SpriteRenderer>();
            entityTransform = GetComponentInParent<Transform>();
        }

        protected override void Start()
        {
            if (shadowOn)
            {
                shadowFromPool = CreateShadow();
                initialShadowScale = shadowFromPool.transform.localScale;
            }
        }

        public override void LogicUpdate()
        {
            if (shadowOn)
            {
                UpdateShadow();
            }
        }


        public void CreateAfterImage(float distanceBetweenImages)
        {
            if (Mathf.Abs(entityTransform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AfterImageSprite afterImage = PoolManager.Instance
                    .GetFromPool<AfterImageSprite>(afterImagePrefab.gameObject, entityTransform.position, entityTransform.rotation);

                if (afterImage != null)
                {
                    afterImage.Initialize(entitySpriteRenderer, alphaBegin, colorLooseRate);
                    afterImage.SetActive(true);

                    lastImageXpos = entityTransform.position.x;
                }
            }
        }

        private Shadow CreateShadow()
        {
            Shadow shadow = PoolManager.Instance.GetFromPool<Shadow>(shadowPrefab.gameObject, entityTransform.position, entityTransform.rotation);

            if (shadow != null)
            {
                shadow.Initialize(entitySpriteRenderer);
            }
            return shadow;
        }

        private void UpdateShadow()
        {
            Vector2 position = core.Sensor.GroundHit.point;
            shadowFromPool.transform.SetPositionAndRotation(position, entityTransform.rotation);

            if (core.Sensor.IsGroundDetect())
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

                if (float.IsNegative(scaleChange.y) || float.IsNegative(scaleChange.x)) return;
                shadowFromPool.transform.localScale = scaleChange;
            }
        }

        public void ShadowIsActive(bool isActive)
        {
            shadowOn = isActive;

            if (!isActive)
            {
                shadowFromPool.ReturnToPool();
            }

            if (isActive && !shadowFromPool.isActiveAndEnabled)
            {
                CreateShadow();
            }
        }

        public AnimationFX<T> CreateAnimationFX<T>(T animationTypeFX, Vector2 offset = default)
            where T : Enum
        {
            AnimationFX<T> animationFx = PoolManager.Instance.GetFromPool<AnimationFX<T>>(GetAnimationGameObject(animationTypeFX), entityTransform.position, entityTransform.rotation);

            if (animationFx != null)
            {
                animationFx.Initialize(animationTypeFX, entityTransform, offset);
                animationFx.SetActive(true);
            }
            return animationFx;
        }

        public void CreateAnimationFX<T>(T animationTypeFX, Vector2 position, Quaternion rotation, bool flipHorizontal = false)
            where T : Enum
        {
            AnimationFX<T> animationFx = PoolManager.Instance.GetFromPool<AnimationFX<T>>(GetAnimationGameObject(animationTypeFX), position, rotation);

            if (flipHorizontal)
            {
                animationFx.transform.Rotate(0.0f, 180, 0.0f);
            }

            if (animationFx != null)
            {
                animationFx.Initialize(animationTypeFX);
                animationFx.SetActive(true);
            }
        }

        private GameObject GetAnimationGameObject(Enum typeFX)
            => typeFX switch
            {
                AbilityFXType => abilityFXPrefab.gameObject,
                DustType => dustPrefab.gameObject,
                _ => throw new NotImplementedException()
            };

    }
}