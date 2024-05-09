﻿
using GXPEngine;
using System;
using System.Collections.Generic;

namespace Physics
{
    public class StaticObj : CollisionInteractor
    {
        public bool Enabled { get; set; } = true;

        public StaticObj(GameObject owner, bool isTrigger = false, float bounciness = 1) : base(owner, isTrigger, bounciness)
        {

        }

        public override void ResolveCollision(CollisionInfo colInfo)
        {

        }

        protected void Update()
        {
            if (isTrigger && Enabled)
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

        public List<Collider> GetOverlaps()
        {
            return engine.GetOverlaps(myCollider);
        }

        public List<Collider> GetSolidOverlaps()
        {
            return engine.GetSolidOverlaps(myCollider);
        }

    }
}
