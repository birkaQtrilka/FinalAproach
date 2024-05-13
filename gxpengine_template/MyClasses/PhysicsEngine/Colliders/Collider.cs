using GXPEngine;
namespace Physics {

	/// <summary>
	/// A superclass for all shapes in our physics engine (like circles, lines, AABBs, ...)
	/// </summary>
	public abstract class Collider 
	{
		public GameObject rbOwner { get; }
		public Vec2 position { get; set; }

		public Collider(CollisionInteractor pOwner, Vec2 startPosition)
		{
			rbOwner=pOwner;
			position=startPosition;
		}
		public abstract CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity);

		public abstract bool Overlaps(Collider other);

		public abstract bool ContainsPoint(Vec2 p);
	}
}
