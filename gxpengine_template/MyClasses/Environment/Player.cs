using GXPEngine;
using GXPEngine.Core;
using gxpengine_template.MyClasses.PickUps;
using gxpengine_template.MyClasses.TankGame;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Player : AnimationSprite, ITrigger
    {
        MovingBall _rigidBody;
        PickUper _pickUper;

        public event Action<GameObject> TriggerStay;
        bool shot;
        public Player(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true, false)
        {
            _pickUper = new PickUper(this);

            AddChild(_pickUper);
            AddChild(new Coroutine(Start(data)));
        }

        IEnumerator Start(TiledObject data)
        {
            yield return null;
            float vx = data.GetFloatProperty("StartX", 1);
            float vy = data.GetFloatProperty("StartY", 1);
            _rigidBody = new MovingBall(this, new Vec2(vx,vy), new Vec2(x, y), width / 2);
            _rigidBody.Drag = data.GetFloatProperty("Drag", .98f);
        }

        void Update()
        {
            if (shot) foreach (Physics.Collider col in _rigidBody.GetOverlaps()) TriggerStay?.Invoke(col.owner);
        }

        public void Shoot(Vec2 velocity)
        {
            _rigidBody.velocity = velocity;
            shot = true;
        }

    }
}
