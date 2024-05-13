using GXPEngine;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class ExitGameBtn : Button, IPrefab
    {
        public ExitGameBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {

        }


        protected override void OnButtonPress()
        {
            MyGame.main.Destroy();
        }
        
        public GameObject Clone()
        {
            return new ExitGameBtn(texture.filename, _cols, _rows, null);
        }
    }
}
