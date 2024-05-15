using GXPEngine;
using gxpengine_template.MyClasses.Tweens;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class MenuStarsUI : TiledGameObject, IPrefab
    {
        public int Score { get; set; }
        int currScore = 0;
        readonly UIStar[] _scoreVisual;
        readonly TiledObject _data;

        public MenuStarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _data = data;
            _scoreVisual = new UIStar[3];
            string enmptyStarName = data.GetStringProperty("EmptyStarName", "Assets/StarC.png");
            string fullStarName = data.GetStringProperty("FullStarName", "Assets/StarC.png");
            float starScaling = data.GetFloatProperty("StarScaling",1);
            for (int i = 0; i < 3; i++)
            {
                var star = new UIStar(new AnimationSprite(enmptyStarName, 3, 1, -1, true, false), new AnimationSprite(fullStarName, 3, 1, -1, true, false), starScaling); ;
                _scoreVisual[i] = star;
                AddChild(star);
                star.SetXY(star.FullSprite.width * i, 0);
            }
            alpha = 0;
        }

        public void PlayAnim()
        {
            //can be for individual stars
            if (++currScore > Score) return;
            UIStar star = _scoreVisual[currScore - 1];
            star.Fill();

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
