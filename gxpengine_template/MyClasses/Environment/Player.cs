using GXPEngine;
using GXPEngine.Core;
using gxpengine_template.MyClasses.PickUps;
using gxpengine_template.MyClasses.TankGame;
using gxpengine_template.MyClasses.Tweens;
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
        bool fallen;

        public Player(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true, false)
        {
            _pickUper = new PickUper(this);
            AddChild(new AnimationCycler(this, data.GetIntProperty("AnimationDelayMs", 500)));

            AddChild(_pickUper);
            AddChild(new Coroutine(Start(data)));
        }

        IEnumerator Start(TiledObject data)
        {
            yield return null;
            RigidBody = new MovingBall(this, Vec2.zero, new Vec2(x, y), width / 2);
            RigidBody.Drag = data.GetFloatProperty("Drag", .98f);
            StartPos = this.GetPosInVec2();
            SetIdleMode();
        }

        void Update()
        {
            if (shot) foreach (Physics.Collider col in RigidBody.GetOverlaps()) TriggerStay?.Invoke(col.rbOwner.parent);
            
            if (RigidBody == null) return;
            
            if(!fallen && !Table.Instance.OnTable(RigidBody.Collider.position))
            {
                AddChild(new Tween(TweenProperty.scale, 500, -1, EaseFunc.EaseInSine).OnCompleted(RestartGame) );
                fallen = true;
            }
        }

        void RestartGame()
        {
            GameManager.Instance.StartIdleMode();
            fallen = false;
            scale = 1;
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
            RigidBody.Collider.position = StartPos;

            shot = false;
        }

        public void SetPlayMode()
        {
            RigidBody.Enabled = true;

        }

    }
}
