using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CoreSystem.Components
{
    public sealed class Movement : CoreComponent
    {
        [SerializeField]
        private float timeIgnoreCollisions;
        [SerializeField] 
        private TilemapCollider2D oneWayPlatformCollider;
        private Collider2D entityCollider;

        private new Rigidbody2D rigidbody;
        public int FacingDirection { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        private Vector2 workingVector;
        private float defaultGravityScale; 

        protected override void Awake()
        {
            base.Awake();
            FacingDirection = 1;
        }

        protected override void Start()
        {
            base.Start();
            rigidbody = GetComponentInParent<Rigidbody2D>();
            entityCollider = GetComponentInParent<Collider2D>();
            defaultGravityScale = rigidbody.gravityScale;
		}

        public override void LogicUpdate() => CurrentVelocity = rigidbody.velocity;

        #region Movement
        public void Move(float velocity, int directX)
        {
            MoveAlongSurface(velocity, directX);

            if (directX == 0) FreezePosOnSlope();         
            else ResetFreezePos();          
        }

        public void MoveAlongSurface(float velocity, int directX)
        {
            if (core.Sensor.IsGroundSlope()) SetVelocityOnSlope(velocity * directX);           
            else SetVelocityOnSmooth(velocity * directX);            
        }
        #endregion

        #region Velocity
        public void SetVelocityOnSlope(float velocity)
        {
            Vector2 perperdicular = core.Sensor.GetGroundPerperdicular();
            workingVector.Set(- velocity * perperdicular.x, - velocity * perperdicular.y);
            SetFinalVelocity();
        }

        public void SetVelocityOnSmooth(float velocity)
        {
            workingVector.Set(velocity, Vector2.zero.y);
            SetFinalVelocity();
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workingVector.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }

        public void SetVelocityX(float velocity)
        {           
            workingVector.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }

        public void SetVelocityY(float velocity)
        {
            workingVector.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }

        public void SetVelocityZero()
        {
            workingVector = Vector2.zero;
            SetFinalVelocity();
        }

        public void SetFinalVelocity()
        {
            rigidbody.velocity = workingVector;
            CurrentVelocity = workingVector;
        }
        #endregion

        #region Flip
        public void FlipToDirection(int xInput)
        {
            if (xInput != Vector2Int.zero.x && xInput != FacingDirection) 
                Flip(); 
        }

        public void Flip()
        {
            FacingDirection *= -1;
            rigidbody.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        #endregion

        #region Freeze Position
        public void FreezePosOnSlope()
        {
            if (core.Sensor.IsGroundSlope())
                FreezePosX();                      
        }

        public void FreezePosX()
        {
            if (rigidbody.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX)) return;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public void FreezePosY()
        {
            if (rigidbody.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY)) return;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        public void ResetFreezePos()
        {
            if (rigidbody.constraints == RigidbodyConstraints2D.FreezeRotation) return;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        #endregion

        #region Gravitation
        public void GravitationOn() => rigidbody.gravityScale = defaultGravityScale;
		public void GravitationOff() => rigidbody.gravityScale = 0.0f;
        #endregion

        #region One Way Platform
        public void LeaveOneWayPlatform()
        {
            IgnoreOneWayPlatform(true);
            StartCoroutine(StopIgnoringOneWayPlatform(timeIgnoreCollisions));
        }
        public void IgnoreOneWayPlatform(bool isIgnore)
        {
            if (Physics2D.GetIgnoreCollision(oneWayPlatformCollider, entityCollider) == isIgnore) return;   
            Physics2D.IgnoreCollision(oneWayPlatformCollider, entityCollider, isIgnore);           
        }

        private IEnumerator StopIgnoringOneWayPlatform(float delay)
        {
            yield return new WaitForSeconds(delay);
            IgnoreOneWayPlatform(false);
        }
        #endregion
    }
}
