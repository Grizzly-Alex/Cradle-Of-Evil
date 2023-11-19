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
        [SerializeField]
        private float colorLooseRate;
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        private float lastImageXpos;


        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
            afterImagePrefab.GetComponent<AfterImageSprite>().SetValues(spriteRenderer, alphaBegin, colorLooseRate); ;
        }


        protected override void Start()
        {
            PoolManger.Instance.CreatePool(afterImagePrefab, afterImagePreload);
        }

        public void CreateAfterImage(float distanceBetweenImages)
        {
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                PoolManger.Instance.GetFromPool(afterImagePrefab, transform.position, transform.rotation);
                lastImageXpos = transform.position.x;
            }
        }
    }
}