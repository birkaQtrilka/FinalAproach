
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class StarsUI : TiledGameObject
    {
        public static StarsUI Instance;

        public StarsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            if(Instance != null && Instance == this)
            {
                Destroy();
                return;
            }
            else
            {
                Instance = this;
            }
        }

        public void Fill()
        {
            SetFrame(++currentFrame);
        }

        protected override void OnDestroy()
        {
            Instance = null;
        }
    }
}
