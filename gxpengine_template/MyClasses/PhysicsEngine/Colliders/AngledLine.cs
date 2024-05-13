using GXPEngine;
using System;

namespace Physics
{
    public class AngledLine : Collider
    {
        public Vec2 Start { get; }
        public Vec2 End { get; }

        public AngledLine(GameObject pOwner, Vec2 start, Vec2 end) : base(pOwner, Vec2.Lerp(start, end, .5f))
        {
            Start = start;
            End = end;
        }

        public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        {
            return null;
        }

        public override bool Overlaps(Collider other)
        {
            if(other is Rectangle rect)
            {
                return ColliderManager.RectLineOverlap(rect, this);
            }
            else if(other is Circle circle)
            {
                return ColliderManager.CircleLineOverlap(circle, this);
            }
            else if(other is AngledLine line)
            {
                return ColliderManager.LineLineOverlap(this, line);
            }
            else
                throw new NotImplementedException();
        }

    }
}
