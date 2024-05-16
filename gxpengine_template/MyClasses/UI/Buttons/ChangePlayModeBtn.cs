using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.Environment;
using TiledMapParser;
using GXPEngine;
using System.Collections;
using System.Xml.Linq;

namespace gxpengine_template.MyClasses.UI
{
    public class ChangePlayModeBtn : Button
    {

        public ChangePlayModeBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            AddChild(new Coroutine(Init(data)));
        }

        IEnumerator Init(TiledObject data)
        {
            yield return null;

            if (GameManager.Instance != null && data.GetBoolProperty("Disapear", false))
                GameManager.Instance.OnWinScreen += OnWinScreen;

        }

        void OnWinScreen()
        {
            parent.RemoveChild(this);

        }

        protected override void OnButtonPress()
        {
            GameManager.Instance.StartIdleMode();
        }
        protected override void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnWinScreen -= OnWinScreen;

        }
    }
}
