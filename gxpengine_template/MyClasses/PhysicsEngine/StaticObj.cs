
using GXPEngine;

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
                foreach (var overlap in engine.GetOverlaps(myCollider))
                {
                    OnTrigger(overlap);
                }

            }
        }

    }
}
