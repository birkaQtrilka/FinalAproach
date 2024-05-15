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
        // Sounds
        Sound _collisionSound;
        SoundChannel _soundChannel;
        float _volume;

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

            // Sounds
            _collisionSound = new Sound(data.GetStringProperty("SoundFileName", "Assets/Sounds/BUmp in to da wall.wav"));
            _volume = data.GetFloatProperty("Volume");

            AddChild(_pickUper);
            AddChild(new Coroutine(Start(data)));
        }

        IEnumerator Start(TiledObject data)
        {
            yield return null;
            
            RigidBody = new MovingBall(this, Vec2.zero, new Vec2(x, y), width / 2);
            RigidBody.Collided += OnCollision;
            RigidBody.Drag = data.GetFloatProperty("Drag", .98f);
            StartPos = this.GetPosInVec2();
            SetIdleMode();
        }

        private void OnCollision(Physics.CollisionInfo info)
        {
            _soundChannel = _collisionSound.Play(volume: _volume);
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

        protected override void OnDestroy()
        {
            RigidBody.Collided -= OnCollision;
        }
    }
}
