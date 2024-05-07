using GXPEngine;
using System;

namespace gxpengine_template
{
    public class Animation
    {
        //fires only on the reset of loop, not on animation end
        public event Action AnimationLoopEnd;
        public event Action AnimationExit;
        readonly AnimationSprite _animSprite;
        readonly int _startFrame;
        readonly int _endFrame;
        //a pause before entering a new animation
        readonly int _exitTime;
        readonly bool _loop;
        bool _endedAnimation;
        int _currExitTime;
        //context is just a container object
        public Animation(Sprite context, AnimationSprite animSprite, int startFrame, int frames, byte animDelay, bool loop = true, int exitTime = 750) 
        {
            animSprite.SetCycle(startFrame, frames, animDelay);
            context.AddChild(animSprite);
            animSprite.SetOrigin(animSprite.width / 2, animSprite.height / 2);
            animSprite.SetXY(0, -animSprite.height / 2 + context.height / 2);
            animSprite.visible = false;

            _startFrame = startFrame;
            _endFrame = frames + startFrame;
            _loop = loop;
            _animSprite = animSprite;
            _exitTime = exitTime;
            _currExitTime = exitTime;
        }
        

        public void Update()
        {
            if (!_endedAnimation && !_loop && _animSprite.currentFrame == _endFrame - 1)
            {
                _currExitTime -= Time.deltaTime;
                if (_currExitTime > 0) return;// prevents calling multiple times endAnim
                    
                EndAnim();
                AnimationLoopEnd?.Invoke();
                return;
            }
            _animSprite.Animate();
        }

        public void StartAnim()
        {
            _endedAnimation = false;
            _animSprite.SetFrame(_startFrame);
            _animSprite.visible = true;
            _currExitTime = _exitTime;
        }

        public void EndAnim()
        {
            AnimationExit?.Invoke();
            _endedAnimation = true;
            _animSprite.visible = false;
        }

        public void Mirror(bool horizontally, bool vertically)
        {
            _animSprite.Mirror(horizontally, vertically);
        }
    }
}
