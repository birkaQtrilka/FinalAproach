using GXPEngine;
using gxpengine_template.MyClasses.Tweens;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class MenuStarsUI : TiledGameObject, IPrefab
    {
        public int Score { get; set; }
        int currScore = 0;
        readonly AnimationSprite[] _scoreVisual;
        readonly TiledObject _data;

        public MenuStarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _data = data;
            _scoreVisual = new AnimationSprite[3];
            string starName = data.GetStringProperty("StarName");
            for (int i = 0; i < 3; i++)
            {
                var star = new AnimationSprite(starName, 2, 1, -1, true, false);
                _scoreVisual[i] = star;
                AddChild(star);
                star.SetXY(star.width * i, 0);
            }
            alpha = 0;
        }

        public void PlayAnim()
        {
            //can be for individual stars
            if (++currScore > Score) return;
            AnimationSprite star = _scoreVisual[currScore - 1];
            star.SetFrame(1);

            star.AddChild
                (
                new Tween(TweenProperty.scale, 300, .3f, EaseFunc.Factory("EaseInSine"))
                .OnCompleted(() =>
                 star.AddChild
                 (
                     new Tween(TweenProperty.scale, 300, -.3f, EaseFunc.Factory("EaseInSine"))
                 .OnCompleted(PlayAnim))
                 )
            );
        }

        public GameObject Clone()
        {
            return new MenuStarsUI(texture.filename, _cols, _rows, _data);
        }
    }
}
