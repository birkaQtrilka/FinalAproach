using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template.MyClasses.ParticleSystem
{
    //public class Emitter : GameObject
    //{
    //    Stack<Particle> _objectPool = new Stack<Particle>();
    //    Vec2 _startVelocity;
    //    Vec2 _acceleration;

    //    int _currTime;
    //    int _particleTimeDensity;

    //    public Emitter(string fileName, int lifeTimeMs, BlendMode blendMode) 
    //    {
            
    //    }

    //    void Update()
    //    {
    //        if(--_currTime < 0)
    //        {
    //            _currTime = _particleTimeDensity;
    //            Particle particle = GetParticle();
    //            MyGame.main.AddChild(particle);
    //            //particle.
    //        }
    //    }


    //    Particle GetParticle()
    //    {
    //        return _objectPool.Pop();
    //    }

    //    void ReleaseParticle(Particle particle) 
    //    {
    //        _objectPool.Push(particle);
    //    }

    //}
}
