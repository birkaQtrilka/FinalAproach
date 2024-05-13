using GXPEngine;
using System.Collections.Generic;

namespace Physics {
	// This class holds our own colliders - similar to the GXPEngine's CollisionManager, but
	//  we can put any shape here.
	// Using MoveUntilCollision, you can move colliders (even those that aren't registered in this class!),
	//  while checking against collisions with the registered colliders. 
	public class ColliderManager {
		public static ColliderManager main {
			get {
				if (_main==null) {
					_main=new ColliderManager();
				}
				return _main;
			}
		}
		static ColliderManager _main;

		List<Collider> solidColliders;
		List<Collider> triggerColliders;

		public ColliderManager() {
			solidColliders=new List<Collider>();
			triggerColliders=new List<Collider>();
		}

		public void AddSolidCollider(Collider col) {
            solidColliders.Add(col);
		}

		public void RemoveSolidCollider(Collider col) {

            solidColliders.Remove(col);
		}

		public void AddTriggerCollider(Collider col) {
			triggerColliders.Add(col);
		}

		public void RemoveTriggerCollider(Collider col) {

            triggerColliders.Remove(col);
		}

		// Note: feel free to do something smart here like space partitioning, to improve efficiency:

		public CollisionInfo MoveUntilCollision(Collider col, Vec2 velocity) {
			CollisionInfo firstCollision = null;
			foreach (Collider other in solidColliders) {
				if (other!=col) {
					CollisionInfo colInfo = col.GetEarliestCollision(other, velocity);
					if (colInfo!=null && colInfo.timeOfImpact<1) {
						if (firstCollision==null || firstCollision.timeOfImpact>colInfo.timeOfImpact) {
							firstCollision=colInfo;
						}
					}
				}
			}
			return firstCollision;
		}

		//public List<Collider> GetTriggerOverlaps(Collider col) {
		//	List<Collider> overlaps = new List<Collider>();
		//	foreach (Collider other in triggerColliders) {
		//		if (other!=col && col.Overlaps(other)) {
		//			overlaps.Add(other);
		//		}
		//	}
		//	return overlaps;
		//}

		public List<Collider> GetSolidOverlaps(Collider col)
		{
			List<Collider> overlaps = new List<Collider>();
			foreach (Collider other in solidColliders)
			{
				if (other != col && col.Overlaps(other))
				{
					overlaps.Add(other);
				}
			}
			return overlaps;
		}

		public List<Collider> GetOverlaps(Collider col)
        {
            List<Collider> overlaps = new List<Collider>();
            foreach (Collider other in triggerColliders)
            {
                if (other != col && col.Overlaps(other))
                {
                    overlaps.Add(other);
                }
            }
            foreach (Collider other in solidColliders)
            {
                if (other != col && col.Overlaps(other))
                {
                    overlaps.Add(other);
                }
            }
            return overlaps;
        }

		public static bool RectCircleOverlap(Rectangle r, Circle c)
		{
            var nearPoint = new Vec2(Mathf.Clamp(c.position.x, r.position.x - r.Radius, r.position.x + r.Radius), Mathf.Clamp(c.position.y, r.position.y - r.Radius, r.position.y + r.Radius));

            Vec2 rayToNearest = nearPoint - c.position;
            float overlap = c.Radius - rayToNearest.Length;
            return overlap > 0;
        }

		public static bool RectLineOverlap(Rectangle r, AngledLine l)
		{
            float x1 = l.Start.x;
            float y1 = l.Start.y;
            float x2 = l.End.x;
            float y2 = l.End.y;
            float rx = r.position.x - r.Radius;
            float ry = r.position.y - r.Radius;
            float rh = r.Radius * 2;
            float rw = r.Radius * 2;

            return LineLineOverlap(x1, y1, x2, y2, rx, ry, rx, ry + rh)
                || LineLineOverlap(x1, y1, x2, y2, rx + rw, ry, rx + rw, ry + rh)
                || LineLineOverlap(x1, y1, x2, y2, rx, ry, rx + rw, ry)
                || LineLineOverlap(x1, y1, x2, y2, rx, ry + rh, rx + rw, ry + rh)
                ||
                (x1 >= rx && x1 <= rx + rw && x2 >= rx && x2 <= rx + rw
                && y1 >= ry && y1 <= ry + rh && y2 >= ry && y2 <= ry + rh)
                ;
        }

        public static bool LineLineOverlap(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {

            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
        }

        public static bool LineLineOverlap(AngledLine l1, AngledLine l2)
        {
			return LineLineOverlap(l1.Start.x, l1.Start.y, l1.End.x, l1.End.y, l2.Start.x, l2.Start.y, l2.End.x, l2.End.y);
        }

        public static bool CircleLineOverlap(Circle c, AngledLine l)
		{
            Vec2 startToCircle = c.position - l.Start;
            Vec2 lineDir = l.End - l.Start;
            Vec2 lineNormal = lineDir.Normal();
            float distanceToLine = Mathf.Abs(startToCircle.Dot(lineNormal));

            float lineProjection = startToCircle.Dot(lineDir.Normalized());
            return distanceToLine < c.Radius && lineProjection >= 0 && lineProjection <= lineDir.Length;
        }
    }
}