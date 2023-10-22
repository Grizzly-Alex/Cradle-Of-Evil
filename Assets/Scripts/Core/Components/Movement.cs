using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class Movement : CoreComponent
    {
        public Rigidbody2D Rigidbody { get; private set; }
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
            Rigidbody = GetComponentInParent<Rigidbody2D>();
            defaultGravityScale = Rigidbody.gravityScale;

		}

        public override void LogicUpdate() => CurrentVelocity = Rigidbody.velocity;

        #region Movement
        public void Move(float velocity, int directX)
        {
            MoveAlongSurface(velocity, directX);

            if (directX == 0) FreezePosOnSlope();         
            else ResetFreezePos();          
        }

        public void MoveAlongSurface(float velocity, int directX)
        {
            if (core.Sensor.GetGroundSlopeAngle() != 0.0f) SetVelocityOnSlope(velocity * directX);           
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
            Rigidbody.velocity = workingVector;
            CurrentVelocity = workingVector;
        }
        #endregion

        #region Flip
        public void FlipToMovement(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection) 
                Flip(); 
        }

        public void Flip()
        {
            FacingDirection *= -1;
            Rigidbody.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        #endregion

        #region Freeze Position
        public void FreezePosOnSlope()
        {
            if (core.Sensor.GetGroundSlopeAngle() == 0.0f) return;
            FreezePosX();          
        }

        public void FreezePosX()
        {
            if (Rigidbody.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX)) return;
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public void FreezePosY()
        {
            if (Rigidbody.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY)) return;
               Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        public void ResetFreezePos()
        {
            if (Rigidbody.constraints == RigidbodyConstraints2D.FreezeRotation) return;
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        #endregion

        #region Gravitation
        public void GravitationOn() => Rigidbody.gravityScale = defaultGravityScale;
		public void GravitationOff() => Rigidbody.gravityScale = 0.0f;
		#endregion
	}
}
