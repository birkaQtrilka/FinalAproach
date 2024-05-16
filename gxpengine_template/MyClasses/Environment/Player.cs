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
        public event Action<GameObject> TriggerStay;

        public Vec2 StartPos { get; set; }
        public MovingBall RigidBody { get; private set; }

        PickUper _pickUper;
        bool shot;
        bool fallen;

        Sound _deathSound;
        float _deathSoundVolume;
        Sound _collisionSound;
        float _collisionVolume;
        Sound _respawnSound;
        float _respawnVolume;
        Sound _bounceSound;
        float _bounceVolume;

        public Player(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true, false)
        {
            _pickUper = new PickUper(this);
            AddChild(new AnimationCycler(this, data.GetIntProperty("AnimationDelayMs", 500)));

            // Sounds
            _collisionSound = new Sound(data.GetStringProperty("CollisionSound"));
            _collisionVolume = data.GetFloatProperty("CollisionVolume");
            _deathSound = new Sound(data.GetStringProperty("DeathSoundName"));
            _deathSoundVolume = data.GetFloatProperty("DeathSoundVolume", 1);
            _respawnSound = new Sound(data.GetStringProperty("RespawnSound"));
            _respawnVolume = data.GetFloatProperty("RespawnVolume", 1);
            _bounceSound = new Sound(data.GetStringProperty("BounceSound"));
            _bounceVolume = data.GetFloatProperty("BounceVolume", 1);

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

        void OnCollision(Physics.CollisionInfo info)
        {
            Console.WriteLine(info.other.parent.name);
            if (info.other.parent.name == "BounceBlock")
                _bounceSound.Play(volume: _bounceVolume);
            else
                _collisionSound.Play(volume: _collisionVolume);
        }

        void Update()
        {
            if (shot) foreach (Physics.Collider col in RigidBody.GetOverlaps()) TriggerStay?.Invoke(col.rbOwner.parent);
            
            if (RigidBody == null) return;
            
            if(!fallen && !Table.Instance.OnTable(RigidBody.Collider.position))
            {
                AddChild(new Tween(TweenProperty.scale, 500, -1, EaseFunc.EaseInSine).OnCompleted(RestartGame) );
                _deathSound.Play(volume:_deathSoundVolume);
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
            _respawnSound.Play(volume: _respawnVolume);

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
