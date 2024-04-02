using Pool;
using Pool.ItemsPool;
using System;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer spriteRenderer;

        [Header("AFTER IMAGE")]
        [SerializeField] 
        private AfterImageSprite afterImagePrefab;
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        [SerializeField]
        private float colorLooseRate;
        private float lastImageXpos;

        [Header("DUST")]
        [SerializeField]
        private Dust dustPrefab;


        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        public void CreateAfterImage(float distanceBetweenImages)
        {
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AfterImageSprite afterImage = PoolManager.Instance
                    .GetFromPool<AfterImageSprite>(afterImagePrefab.gameObject, transform.position, transform.rotation);

                if (afterImage != null)
                {
                    afterImage.Initialize(spriteRenderer, alphaBegin, colorLooseRate);
                    afterImage.SetActive(true);

                    lastImageXpos = transform.position.x;
                }
            }
        }

        public AnimationFX<T> CreateAnimationFX<T>(T animationTypeFX, Transform transform, Vector2 offset) 
            where T : Enum
        {
            AnimationFX<T> animationFx = PoolManager.Instance.GetFromPool<AnimationFX<T>>(dustPrefab.gameObject, transform.position, transform.rotation);

            if (animationFx != null)
            {
                animationFx.Initialize(animationTypeFX, transform, offset);
                animationFx.SetActive(true);
            }
            return animationFx;
        }

        public void CreateAnimationFX<T>(T dustType, Vector2 position, Quaternion rotation, bool flipHorizontal = false)
            where T : Enum
        {
            AnimationFX<T> animationFx = PoolManager.Instance.GetFromPool<AnimationFX<T>>(dustPrefab.gameObject, position, rotation);

            if (flipHorizontal)
            {
                animationFx.transform.Rotate(0.0f, 180, 0.0f);
            }

            if (animationFx != null)
            {
                animationFx.Initialize(dustType);
                animationFx.SetActive(true);
            }
        }

    }
}