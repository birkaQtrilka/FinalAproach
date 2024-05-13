
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

        protected override void ResolveCollision(CollisionInfo colInfo)
        {

        }

        protected void Update()
        {
            if (isTrigger && Enabled)
            {
                CheckForTriggers();

            }
        }

        public void UpdateColliderPosition(float x, float y)
        {
            myCollider.position = new Vec2(x, y);
        }

        public List<Collider> GetSolidOverlaps()
        {
            return engine.GetSolidOverlaps(myCollider);
        }

    }
}
