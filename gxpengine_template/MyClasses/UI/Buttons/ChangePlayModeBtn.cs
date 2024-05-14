using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.Environment;
using TiledMapParser;
using GXPEngine;
using System.Collections;

namespace gxpengine_template.MyClasses.UI
{
    public class ChangePlayModeBtn : Button
    {

        public ChangePlayModeBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
        }

        protected override void OnButtonPress()
        {
            GameManager.Instance.StartIdleMode();
        }
    }
}
