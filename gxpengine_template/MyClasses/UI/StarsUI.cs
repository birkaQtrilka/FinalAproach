
using gxpengine_template.MyClasses.Environment;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class StarsUI : TiledGameObject
    {
        Star[] _stars;
        public static int Score {  get; private set; }

        public StarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            AddChild(new Coroutine(Init()));
            Score = 0;
        }

        IEnumerator Init()
        {
            yield return null;
            _stars = MyGame.main.FindObjectsOfType<Star>();
            foreach (Star star in _stars) {
                star.Grabbed += OnStarGrabbed;
            }
        }

        private void OnStarGrabbed(Star star)
        {
            star.Grabbed -= OnStarGrabbed;
            SetFrame(currentFrame + 1);
            Score++;
        }

        protected override void OnDestroy()
        {
            foreach (Star star in _stars)
                if(star != null)
                    star.Grabbed -= OnStarGrabbed;
        }
    }
}
