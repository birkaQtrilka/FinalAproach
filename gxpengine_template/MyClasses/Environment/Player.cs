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
        public Vec2 StartPos { get; set; }
        public MovingBall RigidBody { get; private set; }
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
            RigidBody = new MovingBall(this, new Vec2(vx,vy), new Vec2(x, y), width / 2);
            RigidBody.Drag = data.GetFloatProperty("Drag", .98f);
            StartPos = this.GetPosInVec2();
            SetIdleMode();
        }

        void Update()
        {
            if (shot) foreach (Physics.Collider col in RigidBody.GetOverlaps()) TriggerStay?.Invoke(col.rbOwner.parent);
                    
                
        }

        public void Shoot(Vec2 velocity)
        {
            RigidBody.Collider.position = StartPos;
            RigidBody.velocity = velocity;
            shot = true;
        }

        public void SetIdleMode()
        {
            RigidBody.Enabled = false;
            shot = false;
        }

        public void SetPlayMode()
        {
            RigidBody.Enabled = true;

        }

    }
}
