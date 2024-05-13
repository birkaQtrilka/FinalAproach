
using gxpengine_template.MyClasses.Environment;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class StarsUI : TiledGameObject
    {
        public Star[] Stars { get; private set; }
        public static int Score {  get; private set; }

        public StarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            AddChild(new Coroutine(Init()));
            ResetScore();
        }

        IEnumerator Init()
        {
            yield return null;
            Stars = MyGame.main.FindObjectsOfType<Star>();
            foreach (Star star in Stars) {
                star.Grabbed += OnStarGrabbed;
            }
        }

        private void OnStarGrabbed(Star star)
        {
            star.Grabbed -= OnStarGrabbed;
            SetFrame(currentFrame + 1);
            Score++;
        }
        
        public void ResetScore()
        {
            SetFrame(0);
            Score = 0;
        }

        protected override void OnDestroy()
        {
            foreach (Star star in Stars)
                if(star != null)
                    star.Grabbed -= OnStarGrabbed;
        }
    }
}
