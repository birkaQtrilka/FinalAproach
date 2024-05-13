using GXPEngine;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class ResetSceneBtn : Button, IPrefab
    {
        public ResetSceneBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {

        }

        public GameObject Clone()
        {
            return new ResetSceneBtn(texture.filename, _cols, _rows, null);
        }

        protected override void OnButtonPress()
        {

            MyUtils.MyGame.ReloadLevel();
        }
    }
}
