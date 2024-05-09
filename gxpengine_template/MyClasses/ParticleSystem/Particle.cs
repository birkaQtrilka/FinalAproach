using GXPEngine;

namespace gxpengine_template.MyClasses.ParticleSystem
{
    public class Particle : Sprite
    {
        public Vec2 Velocity { get; set; }
        public Vec2 Acceleration { get; set; }

        public Particle(string filename) : base(filename, true, false)
        {

        }

        void Update()
        {
            Velocity += Acceleration;
            
            this.AddToPos(Velocity);

        }
    }
}
