using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.Environment;
using TiledMapParser;
using GXPEngine;
using System.Collections;

namespace gxpengine_template.MyClasses.UI
{
    public class ChangePlayModeBtn : Button
    {
        /*temporary for testing*/
        Player _player;
        PlayerLauncher _launcher;
        StarsUI _starsUI;

        public ChangePlayModeBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            _player = MyUtils.MyGame.CurrentLevel.FindObjectOfType<Player>();
            _launcher = MyGame.main.FindObjectOfType<PlayerLauncher>();
            _starsUI = MyGame.main.FindObjectOfType<StarsUI>();
        }

        protected override void OnButtonPress()
        {
            //put this in GameManager?
            _player.SetIdleMode();
            _player.SetPosInVec2(_player.StartPos);
            foreach (var star in _starsUI.Stars)
                star.ReEnable();
            _starsUI.ResetScore();
            Dragger.Instance.CanDrag = true;
            _launcher.CanLaunch = true;
        }
    }
}
