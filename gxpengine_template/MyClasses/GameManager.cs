using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.Environment;
using gxpengine_template.MyClasses.UI;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses
{
    public class GameManager : Sprite
    {
        public event Action OnWinScreen;
        public static GameManager Instance { get; private set; }

        Player _player;
        PlayerLauncher _launcher;
        StarsUI _starsUI;
        NextLevelUI _nextLevelUI;

        public GameManager(TiledObject data) : base("Assets/square.png",true, false)
        {

            if(Instance != null && Instance == this)
            {
                Destroy();
                return;
            }
            else 
                Instance = this;
            visible = false;
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            _player = MyUtils.MyGame.CurrentLevel.FindObjectOfType<Player>();
            _launcher = MyGame.main.FindObjectOfType<PlayerLauncher>();
            _starsUI = MyGame.main.FindObjectOfType<StarsUI>();
            _nextLevelUI = MyGame.main.FindObjectOfType<NextLevelUI>();
        }

        public void StartIdleMode()
        {
            _player.SetIdleMode();
            _player.SetPosInVec2(_player.StartPos);
            foreach (var star in _starsUI.CollectibleStars)
                star.ReEnable();
            _starsUI.ResetScore();
            Dragger.Instance.CanDrag = true;
            _launcher.CanLaunch = true;
        }

        public void StartPlayMode()
        {
            _player.SetPlayMode();
            Dragger.Instance.CanDrag = false;
            _launcher.CanLaunch = false;
        }

        public void SpawnWinScreen()
        {
            OnWinScreen?.Invoke();
            _nextLevelUI.Pop();
            Dragger.Instance.CanDrag = false;
        }

        protected override void OnDestroy()
        {
            OnWinScreen = null;
            Instance = null;
        }
    }
}
