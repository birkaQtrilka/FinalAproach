using GXPEngine;
using System;

namespace Physics
{
    public class Rectangle : Collider
    {
        public Vec2 Size { get; set; }

        public Rectangle(CollisionInteractor pOwner, Vec2 startPosition, Vec2 size) : base(pOwner, startPosition)
        {
            Size = size;
        }

        public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        {
            if(other is Rectangle rect )
            {
                return RectRectCol(rect, position, velocity * (Time.deltaTime * 0.01f));
            }
            else if (other is Circle circle)
            {
                return DiscreteCircleCol(circle, velocity);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public override bool Overlaps(Collider other)
        {
            if (other is Rectangle rect)
                return
                    rect.position.x - rect.Size.x < position.x + Size.x &&
                    rect.position.x + rect.Size.x > position.x - Size.x &&
                    rect.position.y - rect.Size.y < position.y + Size.y &&
                    rect.position.y + rect.Size.y > position.y - Size.y;
            else if (other is Circle circle)
            {
                return ColliderManager.RectCircleOverlap(this, circle);
            }
            else if (other is AngledLine line)
            {
                return ColliderManager.RectLineOverlap(this, line);
            }
            else
                throw new NotImplementedException();
        }
        
        CollisionInfo DiscreteCircleCol(Circle circle, Vec2 velocity)
        {
            Vec2 potentialPos = position + velocity;
            var nearPoint = new Vec2(Mathf.Clamp(circle.position.x, potentialPos.x - Size.x, potentialPos.x + Size.x), Mathf.Clamp(circle.position.y, potentialPos.y - Size.y, potentialPos.y + Size.y));

            Vec2 rayToNearest = nearPoint - circle.position;
            float overlap = circle.Radius - rayToNearest.Length;
            if (overlap > 0)
            {
                Vec2 normal = (position - nearPoint).Normalized();
                Vec2 poi = potentialPos - normal * overlap;
                float t = (position - poi).Length / velocity.Length;
                return new CollisionInfo(normal, circle.rbOwner, t, poi);
            }
            return null;
        }

        public CollisionInfo RectRectCol(Rectangle target, Vec2 rayOrigin, Vec2 rayDir)
        {
            Vec2 contactPoint;
            Vec2 normal;
            float t;

            if (rayDir.x == 0 && rayDir.y == 0) return null;
            Vec2 targtPos = target.position - Size;
            Vec2 targtRadius = new Vec2(target.Size.x, target.Size.y) + Size * 2;

            float nearX = (targtPos.x - rayOrigin.x - target.Size.x) / rayDir.x;
            float farX = (targtPos.x + targtRadius.x - rayOrigin.x) / rayDir.x;

            float nearY = (targtPos.y - rayOrigin.y - target.Size.y) / rayDir.y;
            float farY = (targtPos.y + targtRadius.y - rayOrigin.y) / rayDir.y;

            if (double.IsNaN(nearX) || double.IsNaN(nearY) || double.IsNaN(farX) || double.IsNaN(farY))
                return null;

            if (nearX > farX) (nearX, farX) = (farX, nearX);
            if (nearY > farY) (nearY, farY) = (farY, nearY);
            // at this point, for both x and y, the first tuple element is smaller than the second (so it's a real interval) (???)


            if (nearX > farY || nearY > farX) return null; // the intervals don't overlap

            float hitNear = Mathf.Max(nearX, nearY); // start of overlap is max of both interval starts (???)
            float hitFar = Mathf.Min(farX, farY);
            if (hitFar < 0 || hitNear > 1) return null;
            t = hitNear;
            contactPoint = rayOrigin + hitNear * rayDir;


            if (nearX > nearY) // if already overlapping in y direction and then in x direction (that's where true overlap time starts), then the collision normal is horizontal
            {
                if (rayDir.x < 0)
                    normal = new Vec2(1, 0);
                else
                    normal = new Vec2(-1, 0);
            }
            else 
            {
                if (rayDir.y < 0)
                    normal = new Vec2(0, 1);
                else
                    normal = new Vec2(0, -1);

            }

            return new CollisionInfo(normal,target.rbOwner,t,contactPoint);
        }

        public void DrawSelf()
        {
            Gizmos.DrawRectangle(position.x, position.y, Size.x * 2, Size.y * 2);
        }

        public override bool ContainsPoint(Vec2 p)
        {
            return p.x >= position.x - Size.x * .5f && p.x <= position.x + Size.x * .5f
                            && p.y >= position.y - Size.y * .5f && p.y <= position.y + Size.y * .5f;
        }
    }
}
