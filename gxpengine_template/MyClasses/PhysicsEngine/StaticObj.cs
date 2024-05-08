
using GXPEngine;
using System;
using System.Collections.Generic;

namespace Physics
{
    public class StaticObj : CollisionInteractor
    {

        public StaticObj(GameObject owner, bool isTrigger = false, float bounciness = 1) : base(owner, isTrigger, bounciness)
        {

        }

        public override void ResolveCollision(CollisionInfo colInfo)
        {

        }

        protected void Update()
        {
            if (isTrigger)
            {
                foreach (Collider overlap in engine.GetOverlaps(myCollider))
                {
                    OnTrigger(overlap);
                }

            }
        }

        public void UpdateColliderPosition(float x, float y)
        {
            myCollider.position = new Vec2(x, y);
        }

        public List<Collider> GetTriggerOverlaps()
        {
            return engine.GetOverlaps(myCollider);
        }
    }
}
