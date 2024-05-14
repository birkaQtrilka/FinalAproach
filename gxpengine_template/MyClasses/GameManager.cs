using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.Environment;
using gxpengine_template.MyClasses.UI;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses
{
    public class GameManager : TiledGameObject
    {
        public static GameManager Instance { get; private set; }

        Player _player;
        PlayerLauncher _launcher;
        StarsUI _starsUI;

        public GameManager(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            if(Instance != null && Instance == this)
            {
                Destroy();
                return;
            }
            else 
                Instance = this;
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            _player = MyUtils.MyGame.CurrentLevel.FindObjectOfType<Player>();
            _launcher = MyGame.main.FindObjectOfType<PlayerLauncher>();
            _starsUI = MyGame.main.FindObjectOfType<StarsUI>();
        }

        public void StartIdleMode()
        {
            _player.SetIdleMode();
            _player.SetPosInVec2(_player.StartPos);
            foreach (var star in _starsUI.Stars)
                star.ReEnable();
            _starsUI.ResetScore();
            Dragger.Instance.CanDrag = true;
            _launcher.CanLaunch = true;
        }

        protected override void OnDestroy()
        {
            Instance = null;
        }
    }
}
