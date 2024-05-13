using GXPEngine;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class DisableObjectButton : Button, IPrefab
    {
        public GameObject GameObject { get; set; }

        public DisableObjectButton(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {

        }

        protected override void OnButtonPress()
        {
            GameObject?.parent.RemoveChild(GameObject);
        }

        public GameObject Clone()
        {
            return new DisableObjectButton(texture.filename,_cols,_rows, null);
        }
    }
}
