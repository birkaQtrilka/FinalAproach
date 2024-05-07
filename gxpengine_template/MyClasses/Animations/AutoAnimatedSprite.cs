using GXPEngine;
using TiledMapParser;

namespace gxpengine_template
{
    // Convenient because you don't have to specify to animate every time
    public class AutoAnimatedSprite : AnimationSprite
    {
        public AutoAnimatedSprite(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows)
        {
            _animationDelay = (byte)data.GetIntProperty("AnimationDelay", 8);
            MyGame.main.OnAfterStep += OnAfterStep;
        }

        void OnAfterStep()
        {
            AnimateFixed();
        }

        protected override void OnDestroy()
        {
            MyGame.main.OnAfterStep -= OnAfterStep;
        }

    }
}
