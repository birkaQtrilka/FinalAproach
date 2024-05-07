using GXPEngine;
using gxpengine_template.MyClasses.PickUps;
using TiledMapParser;

namespace gxpengine_template
{
    public abstract class PickUp : AnimationSprite
    {
        protected Sound pickUpSound;
        readonly SoundData _pickUpSoundData;

        public PickUp(string fileName, int c, int r, TiledObject data) : base(fileName, c, r,addCollider: true)
        {
            collider.isTrigger = true;

            if (data == null) return;

            pickUpSound = new Sound(data.GetStringProperty("PickUpSound"));
            _pickUpSoundData = new SoundData
            (
                data.GetFloatProperty("Volume", 0.3f)
            );
            
            SetCycle(0, c, animationDelay: 6);

        }

        public void Take(ITrigger taker) 
        {
            Grab(taker);
            pickUpSound.Play(volume: _pickUpSoundData.volume);
            LateDestroy();
        }

        protected abstract void Grab(ITrigger taker);
    }
}
