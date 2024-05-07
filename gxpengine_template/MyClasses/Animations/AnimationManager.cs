using GXPEngine;
using System.Collections.Generic;

namespace gxpengine_template
{
    public abstract class AnimationManager : GameObject
    {
        //triggers are just a tool to easily activate animations based on priority
        protected readonly struct Trigger
        {
            public readonly string AnimName;
            public readonly byte Priority;

            public Trigger(string animName, byte priority)
            { 
                AnimName = animName;
                Priority = priority;
            }
        }
        protected Dictionary<string, Animation> _animations;
        protected readonly List<Trigger> triggers = new List<Trigger>();
        protected Animation currAnimation;
        protected void TransitionToAnim(Animation newAnim)
        {
            currAnimation.EndAnim();
            currAnimation = newAnim;
            currAnimation.StartAnim();
        }
    }
}
