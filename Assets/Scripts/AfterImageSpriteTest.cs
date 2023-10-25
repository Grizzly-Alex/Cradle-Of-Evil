using UnityEngine;

namespace Assets.Scripts
{
    public sealed class AfterImageSpriteTest : MonoBehaviour
    {
        [SerializeField]
        private float activeTime = 0.1f;

        [SerializeField]
        private float alphaDecay = 0.85f;

        private float timeActivated;
        private float alpha;

        private Transform entity;

        private SpriteRenderer spriteRenderer;
        private SpriteRenderer entitySpriteRenderer;

        [SerializeField]
        private Color color;

        private void Awake()
        {
            //entity = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void OnEnable()
        {
            entity = GameObject.FindGameObjectWithTag("Player").transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
            entitySpriteRenderer = entity.GetComponent<SpriteRenderer>();
            alpha = color.a;
            spriteRenderer.sprite = entitySpriteRenderer.sprite;
            transform.SetPositionAndRotation(entity.position, entity.rotation);
            timeActivated = Time.time;
        }

        private void Update()
        {
            alpha -= alphaDecay * Time.deltaTime;
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);

            if (Time.time >= (timeActivated + activeTime))
            {
                AfterImagePoolTest.Instance.AddToPool(gameObject);
            }

        }

    }
}
