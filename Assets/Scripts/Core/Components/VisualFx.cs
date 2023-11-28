using Pool;
using Pool.ItemsPool;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer spriteRenderer;

        [Header("AFTER IMAGE")]
        [SerializeField] 
        private GameObject afterImagePrefab;
        [SerializeField]
        private int afterImagePreload;
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        [SerializeField]
        private float colorLooseRate;
        private float lastImageXpos;


        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        public void CreateAfterImage(float distanceBetweenImages)
        {
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AfterImageSprite afterImage = PoolManger.Instance.GetFromPool<AfterImageSprite>(afterImagePrefab, transform.position, transform.rotation);

                if (afterImage != null)
                {
                    afterImage.SetValues(spriteRenderer, alphaBegin, colorLooseRate);
                    afterImage.gameObject.SetActive(true);
                }

                lastImageXpos = transform.position.x;
            }
        }
    }
}