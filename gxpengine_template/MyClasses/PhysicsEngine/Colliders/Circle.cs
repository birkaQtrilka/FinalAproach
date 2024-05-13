using GXPEngine;
using System;
namespace Physics
{
    public class Circle : Collider
    {
        public float Radius { get; set; }

        public Circle(CollisionInteractor pOwner, Vec2 startPosition, float radius) : base(pOwner, startPosition)
        {
            Radius = radius;
        }

        public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        {
            if(other is Circle circle)
            {
                 return CircleCollision(circle, velocity);
            }
            else if(other is AngledLine line)
            {
                return LineCollision(line, velocity);
            }
            else if (other is Rectangle rect)
            {
                return DiscreteRectCollision(rect, velocity);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        CollisionInfo DiscreteRectCollision(Rectangle rect, Vec2 velocity)
        {
            var nearPoint = new Vec2(Mathf.Clamp(position.x, rect.position.x - rect.Radius, rect.position.x + rect.Radius), Mathf.Clamp(position.y, rect.position.y - rect.Radius, rect.position.y + rect.Radius));
            Vec2 potentialPos = position + velocity;

            Vec2 rayToNearest = nearPoint - potentialPos;
            float overlap = Radius - rayToNearest.Length;
            if(overlap > 0)
            {
                Vec2 normal = (position - nearPoint).Normalized();
                Vec2 poi = potentialPos + normal * overlap;
                float t = (position - poi).Length / velocity.Length;
                return new CollisionInfo(normal, rect.rbOwner, t, poi);
            }
            return null;
        }

        CollisionInfo LineCollision(AngledLine line, Vec2 velocity)
        {
            Vec2 lineDir = (line.End - line.Start);
            Vec2 lineNormal = lineDir.Normal();

            Vec2 startLineToOldBall = position - line.Start;

            float distanceToLine = startLineToOldBall.Dot(lineNormal) - Radius;//distance from point on circumferance
            float wholeDistance = -velocity.Dot(lineNormal);
            float timeOfImpact;
            if (wholeDistance <= 0) return null; //opposite way facing with back

            if (distanceToLine >= 0)
                timeOfImpact = distanceToLine / wholeDistance;
            else if (distanceToLine > -Radius)//currently colliding
                timeOfImpact = 0;
            else//past the line
                return null;

            if (timeOfImpact > 1) return null;// in the future


            Vec2 pointOfImpact =  position + velocity * timeOfImpact;

            float projectionFromPOI = (pointOfImpact - line.Start).Dot(lineDir.Normalized());//newProjection = d from slides

            if (projectionFromPOI < 0 || projectionFromPOI > lineDir.Length ) return null;

            return new CollisionInfo(lineNormal, line.rbOwner, timeOfImpact, pointOfImpact);
        }

        CollisionInfo CircleCollision(Circle other, Vec2 velocity)
        {
            Vec2 relativePosition = position - other.position;

            float c = Mathf.Pow(relativePosition.Length, 2) - Mathf.Pow(Radius + other.Radius, 2);
            float b = (2 * relativePosition).Dot(velocity);

            if (c < 0)
            {
                if (b >= 0) return null;

                Vec2 normalOfCol = (position - other.position).Normalized();
                return new CollisionInfo(normalOfCol, other.rbOwner, 0, position);
            }

            float a = Mathf.Pow(velocity.Length, 2);

            if (a < 0.000001f) return null;


            float delta = b * b - 4 * a * c;

            if (delta < 0) return null;
            
            float timeOfImpact = (-b - Mathf.Sqrt(delta)) / (2 * a);

            if (timeOfImpact < 0 || timeOfImpact >= 1) return null;

            Vec2 poi = position + velocity * timeOfImpact; // oldPos
            Vec2 normal = (poi - other.position).Normalized();
            return new CollisionInfo(normal, other.rbOwner, timeOfImpact, poi);
        }

        public override bool Overlaps(Collider other)
        {
            if (other is Circle circle)
            {
                 return circle.position.DistanceTo(position) < Radius + circle.Radius;
            }
            else if(other is AngledLine line)
            {
                return ColliderManager.CircleLineOverlap(this, line);
            }
            else if(other is Rectangle rect)
            {
                return ColliderManager.RectCircleOverlap(rect, this);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override bool ContainsPoint(Vec2 p)
        {
            return p.DistanceTo(position) <= Radius;
        }
    }
}
