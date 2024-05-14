
using GXPEngine;
using gxpengine_template.MyClasses.Environment;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class StarsUI : TiledGameObject
    {
        public Star[] CollectibleStars { get; private set; }
        public int Score {  get; private set; }
        readonly AnimationSprite[] _scoreVisual;

        public StarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _scoreVisual = new AnimationSprite[3];
            string starName = data.GetStringProperty("StarName","Assets/StarC.png");
            for (int i = 0; i < 3; i++)
            {
                var star = new AnimationSprite(starName, 2, 1, -1, true, false);
                _scoreVisual[i] = star;
                AddChild(star);
                star.SetXY(star.width * i, 0);
            }
            alpha = 0;
            AddChild(new Coroutine(Init()));
            ResetScore();
        }

        IEnumerator Init()
        {
            yield return null;
            CollectibleStars = MyGame.main.FindObjectsOfType<Star>();
            foreach (Star star in CollectibleStars) {
                star.Grabbed += OnStarGrabbed;
            }
        }

        private void OnStarGrabbed(Star star)
        {
            _scoreVisual[Score].SetFrame(1);
            Score = (int)Mathf.Clamp(Score, 0, 3);
            ++Score;
        }

        public void ResetScore()
        {
            foreach (var score in _scoreVisual)
                score.SetFrame(0);

            Score = 0;
        }

        protected override void OnDestroy()
        {
            foreach (Star star in CollectibleStars)
                if(star != null)
                    star.Grabbed -= OnStarGrabbed;
        }
    }
}
