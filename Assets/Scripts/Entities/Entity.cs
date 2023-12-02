using FiniteStateMachine;
using CoreSystem;
using UnityEngine;
using Interfaces;


namespace Entities
{
    public abstract class Entity : MonoBehaviour, ITriggerAnimation
    {
        protected StateMachine stateMachine;
        public Core Core { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody2D Rigidbody {  get; private set; }
        public CapsuleCollider2D BodyCollider {  get; private set; }   


        protected virtual void Awake()
        {
            stateMachine = new StateMachine();
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
            BodyCollider = GetComponent<CapsuleCollider2D>();        
        }

        protected virtual void Start()
        {
            Core = GetComponentInChildren<Core>();
        }

        protected virtual void Update()
        {
            Core.LogicUpdate();
            stateMachine.CurrentState.Update();           
        }

        public void SetColliderHeight(float height)
        {
            Vector2 offset = BodyCollider.offset;
            Vector2 newSize = new(BodyCollider.size.x, height);
            offset.y += (height - BodyCollider.size.y) / 2;
            BodyCollider.size = newSize;
            BodyCollider.offset = offset;
        }

        public void AnimationFinishTrigger() 
            => stateMachine.CurrentState.AnimationFinishTrigger();

        public void AnimationTrigger() 
            => stateMachine.CurrentState.AnimationTrigger();
    }
}