using GXPEngine;
using GXPEngine.Core;
using Physics;
using System.Drawing;

namespace gxpengine_template.MyClasses.TankGame
{
    public class MovingBall : Mover
    {
        public readonly int Radius;

        public override float Mass
        {
            get
            {
                return Radius * Radius * density;
            }
        }


        public MovingBall(GameObject owner,Vec2 startVelocity, Vec2 startPos, int radius, float density = 1.1f) : base(owner, startVelocity)
        {
            Radius = radius;
            this.density = density;
            SetCollider(new Circle(this, startPos, radius));

        }

        protected override void AfterPhysicsStep()
        {
            //add friction
        }

        public void ApplyImulse(Vec2 force)
        {
            velocity = force;
        }
    }
}
