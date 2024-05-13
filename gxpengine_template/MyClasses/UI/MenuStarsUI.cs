using GXPEngine;
using gxpengine_template.MyClasses.Tweens;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class MenuStarsUI : TiledGameObject, IPrefab
    {
        public int Score { get; set; }
        int currScore = 0;

        public MenuStarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {

        }

        public void PlayAnim()
        {
            //can be for individual stars
            if (++currScore > Score-1) return;
            SetFrame(currentFrame + 1);

            AddChild
                (
                new Tween(TweenProperty.scale, 300, .3f, EaseFunc.Factory("EaseInSine"))
                .OnCompleted(() => 
                 AddChild
                 (
                     new Tween(TweenProperty.scale, 300, -.3f, EaseFunc.Factory("EaseInSine"))
                 .OnCompleted(PlayAnim))
                 )
            );
        }

        public GameObject Clone()
        {
            return new MenuStarsUI(texture.filename,_cols, _rows, null);
        }
    }
}
