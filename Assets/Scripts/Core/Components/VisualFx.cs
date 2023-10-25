using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer spriteRenderer;

        [Header("AFTER IMAGE")]
        [SerializeField] private GameObject afterImagePrefab;
        [SerializeField] private float colorLooseRate;
        [SerializeField] private float afterImageCooldown;
        private float afterImageCooldownTimer;

        protected override void Start()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        public override void LogicUpdate() => afterImageCooldownTimer -= Time.deltaTime;

        public void CreateAfterImage()
        {
            if (afterImageCooldownTimer < 0)
            {
                afterImageCooldownTimer = afterImageCooldown;
                GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
                newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate, spriteRenderer.sprite);
            }
        }
    }
}
