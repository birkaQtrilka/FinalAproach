using GXPEngine;

namespace gxpengine_template.MyClasses
{
    public class AnimationCycler : GameObject
    {
        public int _animationTimer { get; set; }
        int _currTime;
        AnimationSprite _sprite;

        public AnimationCycler(AnimationSprite sprite,int animationTimer)
        {
            _sprite = sprite;
            _animationTimer = animationTimer;
        }

        void Update()
        {
            _currTime -= Time.deltaTime;
            if (_currTime <= 0)
            {
                if (_sprite.currentFrame + 1 >= _sprite.frameCount)
                    _sprite.currentFrame = 0;
                else
                    _sprite.currentFrame++;
                _sprite.SetFrame(_sprite.currentFrame);
                _currTime = _animationTimer;
            }
        }
    }
}
