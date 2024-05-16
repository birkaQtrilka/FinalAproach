using GXPEngine;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class ExitGameBtn : Button, IPrefab
    {
        TiledObject _data;

        public ExitGameBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _data = data;
            AddChild(new Coroutine(Init()));
        }

        protected ExitGameBtn(string filename, int cols, int rows, TiledObject data, Sound clickSound, float clickVolume = 1) : base(filename, cols, rows, data, clickSound, clickVolume)
        {
        }

        IEnumerator Init()
        {
            yield return null;
            if (GameManager.Instance != null && _data.GetBoolProperty("Disapear", false))
                GameManager.Instance.OnWinScreen += OnWinScreen;

        }

        void OnWinScreen()
        {
            parent.RemoveChild(this);
        }

        protected override void OnButtonPress()
        {
            MyGame.main.Destroy();
        }
        
        public GameObject Clone()
        {
            return new ExitGameBtn(texture.filename, _cols, _rows, _data, clickSound,clickVolume);
        }

        protected override void OnDestroy()
        {
            if(GameManager.Instance != null)
                GameManager.Instance.OnWinScreen -= OnWinScreen;

        }
    }
}
