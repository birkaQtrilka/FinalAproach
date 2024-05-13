using GXPEngine;

namespace Physics
{
    public class Mover : CollisionInteractor
    {

        public Vec2 velocity;
        public Vec2 acceleration;
        public float density = 1.1f;
        public float Drag 
        { 
            get=> _drag; 
            set=> _drag = Mathf.Clamp(value,0,1); 
        }
        float _drag;
        public virtual float Mass
        {
            get
            {
                return 10 * density;
            }
        }

        public Vec2 position => new Vec2(x, y);

        public Mover(GameObject owner, Vec2 startVelocity, float bounciness = 1) : base(owner, false, bounciness) 
        {
            velocity = startVelocity;
        }

        protected void Update()
        {
            if (!Enabled) return;

            velocity += acceleration ;

            if(!isTrigger)
            {
                var earliestCollision = ColliderManager.main.MoveUntilCollision(myCollider, velocity);

                if (earliestCollision != null)
                {
                    OnCollision(earliestCollision);
                    ResolveCollision(earliestCollision);
                }
                else
                    myCollider.position += velocity;

            } else
            {
                CheckForTriggers();

                myCollider.position += velocity;

            }

            UpdateScreenPosition();
            
            acceleration = Vec2.zero;
            velocity *= _drag;
            AfterPhysicsStep();
        }

        protected virtual void AfterPhysicsStep()
        {

        }

        void UpdateScreenPosition()
        {
            if (parent == null) return;
            parent.x = myCollider.position.x;
            parent.y = myCollider.position.y;
        }

        protected override void ResolveCollision(CollisionInfo col)
        {
            myCollider.position = col.pointOfImpact;


            if (col.other is Mover otherMover)
            {
                //if velocity is facing same way
                Vec2 relativeVel = velocity - otherMover.velocity;
                float dot = relativeVel.Dot(velocity);
                if (dot < 0) return;

                Vec2 u = VelocityOfCenterOfMass(this, otherMover);
                velocity -= (1 + Bounciness * otherMover.Bounciness) * (velocity - u).Dot(col.normal) * col.normal;
                otherMover.velocity -= (1 + Bounciness * otherMover.Bounciness) * (otherMover.velocity - u).Dot(-col.normal) * -col.normal;
            }
            else
            {
                Vec2 reflection = velocity.Reflect(col.normal, (col.other as CollisionInteractor).Bounciness);
                velocity = reflection;
            }
        }

        Vec2 VelocityOfCenterOfMass(Mover a, Mover b)
        {
            return (a.Mass * a.velocity + b.Mass * b.velocity) / (a.Mass + b.Mass);
        }
    }
}
