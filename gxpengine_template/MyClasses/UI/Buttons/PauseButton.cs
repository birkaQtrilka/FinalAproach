using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class PauseButton : Button
    {
        PauseMenu _pauseMenu;
        public PauseButton(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            _pauseMenu = MyGame.main.FindObjectOfType<PauseMenu>();
            if (GameManager.Instance != null)
                GameManager.Instance.OnWinScreen += OnWinScreen;

        }

        void OnWinScreen()
        {
            parent.RemoveChild(this);

        }

        protected override void OnButtonPress()
        {
            _pauseMenu.Toggle();
        }

        protected override void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnWinScreen -= OnWinScreen;

        }
    }
}
