using NewCoreSystem;
using Pool;
using Pool.ItemsPool;
using System;
using UnityEngine;

namespace NewCore.CoreComponents.VisualFxComponents
{
    [Serializable]
    public class AfterImageFx : VisualFxComponent
    {
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        [SerializeField]
        private float colorLooseRate;
        [SerializeField]
        private GameObject prefab;

        private float lastImageXpos;
        private SpriteRenderer entitySpriteRenderer;

        public AfterImageFx(Core core) : base(core)
        {
            entitySpriteRenderer = core.GetComponentInParent<SpriteRenderer>();
        }

        public void CreateAfterImage(float distanceBetweenImages)
        {
            if (Mathf.Abs(entityTransform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AfterImageSprite afterImage = PoolManager.Instance
                    .GetFromPool<AfterImageSprite>(prefab, entityTransform.position, entityTransform.rotation);

                if (afterImage != null)
                {
                    afterImage.Initialize(entitySpriteRenderer, alphaBegin, colorLooseRate);
                    afterImage.SetActive(true);

                    lastImageXpos = entityTransform.position.x;
                }
            }
        }
    }
}
