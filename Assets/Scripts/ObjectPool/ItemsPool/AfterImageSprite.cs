using System;
using UnityEngine;

namespace ObjectPool.ItemsPool
{
    public sealed class AfterImageSprite : ItemPool<Action<AfterImageSprite>>
    {
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin = 1;
        private float alphaUpdate;
        [SerializeField]
        private float colorLooseRate = 1;

        private Transform entity;
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer entitySpriteRenderer;
        private Color color;


        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            entity = GameObject.FindGameObjectWithTag("Player").transform;
            entitySpriteRenderer = entity.GetComponent<SpriteRenderer>();
            alphaUpdate = alphaBegin;
            color = spriteRenderer.color;
            spriteRenderer.sprite = entitySpriteRenderer.sprite;
            transform.SetPositionAndRotation(entity.position, entity.rotation);
        }


        private void Update()
        {
            alphaUpdate -= colorLooseRate * Time.deltaTime;
            spriteRenderer.color = new Color(color.r, color.g, color.b, alphaUpdate);

            if(spriteRenderer.color.a <= 0)
            {               
                Relise.Invoke(this);
            }
        }
    }
}
