using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class ResetSceneBtn : Button
    {
        public ResetSceneBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {

        }

        protected override void OnButtonPress()
        {

            MyUtils.MyGame.ReloadLevel();
        }
    }
}
