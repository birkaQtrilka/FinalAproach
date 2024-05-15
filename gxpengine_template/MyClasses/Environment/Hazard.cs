using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using Physics;
using System.Collections;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Hazard : TiledGameObject
    {
        StaticObj _trigger;
        float _waitForDeath;
        bool _collided;
        Sound _deathSound;
        float _deathSoundVolume;

        public Hazard(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            SetOrigin(width / 2, height / 2);
            _waitForDeath = data.GetFloatProperty("WaitForDeathS", 0.3f);
            _deathSound = new Sound(data.GetStringProperty("DeathSoundName"));
            _deathSoundVolume = data.GetFloatProperty("DeathSoundVolume", 1);
            AddChild(new AnimationCycler(this, data.GetIntProperty("AnimationTimerMs",500)));
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            _trigger = new StaticObj(this, true);
            _trigger.SetCollider(new Circle(_trigger, new Vec2(x, y), width/2));
            _trigger.Enabled = true;
            _trigger.OnTriggerStay += OnTouch;
        }

        private void OnTouch(Collider obj)
        {
            if (!_collided && obj.rbOwner.parent is Player)
            {
                _collided = true;
                AddChild(new Coroutine(SlightPause())) ;
            }
        }

        IEnumerator SlightPause()
        {
            yield return new WaitForSeconds(_waitForDeath);
            GameManager.Instance.StartIdleMode();
            _deathSound.Play(volume:_deathSoundVolume);
            _collided = false;
        }
    }
}
