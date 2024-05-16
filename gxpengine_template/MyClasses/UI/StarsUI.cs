
using GXPEngine;
using gxpengine_template.MyClasses.Environment;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class UIStar : GameObject
    {
        public AnimationSprite FullSprite => _fullStar;
        AnimationSprite _emptyStar;
        AnimationSprite _fullStar;
        public UIStar(AnimationSprite emptyStar, AnimationSprite fullStar, float scaling = 1)
        {
            _emptyStar = emptyStar;
            _fullStar = fullStar;
            emptyStar.scale = scaling;
            fullStar.scale = scaling;
            emptyStar.AddChild(new AnimationCycler(emptyStar, 500));
            fullStar.AddChild(new AnimationCycler(fullStar, 500));
            AddChild(emptyStar);
        }

        public void Fill()
        {
            RemoveChild(_emptyStar);
            AddChild(_fullStar);
        }

        public void UnFill()
        {
            RemoveChild(_fullStar);
            AddChild(_emptyStar);
        }
    }

    public class StarsUI : TiledGameObject
    {
        public Star[] CollectibleStars { get; private set; }
        public int Score {  get; private set; }
        readonly UIStar[] _scoreVisual;

        public StarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _scoreVisual = new UIStar[3];
            string enmptyStarName = data.GetStringProperty("EmptyStarName","Assets/StarC.png");
            string fullStarName = data.GetStringProperty("FullStarName","Assets/StarC.png");

            for (int i = 0; i < 3; i++)
            {
                var star = new UIStar(new AnimationSprite(enmptyStarName, 3, 1, -1, true, false), new AnimationSprite(fullStarName, 3, 1, -1, true, false));
                _scoreVisual[i] = star;
                AddChild(star);
                star.SetXY(0, star.FullSprite.height * i);
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
            _scoreVisual[Score].Fill();
            Score = (int)Mathf.Clamp(Score, 0, 3);
            ++Score;
        }

        public void ResetScore()
        {
            foreach (var score in _scoreVisual)
                score.UnFill();

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
