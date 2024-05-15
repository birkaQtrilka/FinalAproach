using GXPEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class PauseMenu : TiledGameObject
    {
        readonly NextLevelButton _startScreenBtn;
        readonly DisableObjectButton _resumeBtn;
        readonly ExitGameBtn _exitGameBtn;
        readonly Vec2 _mainScreenBtnPos;
        readonly Vec2 _resumeBtnPos;
        readonly Vec2 _exitBtnPos;
        bool _opened;

        Sound _openSound;
        float _openSoundVolume;
        float _closeSoundFrequency;

        public PauseMenu(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            Dictionary<string, IPrefab> prefabs = MyUtils.MyGame.Prefabs;
            _startScreenBtn = prefabs[data.GetStringProperty("GoToMainScreenBtnName", "NxtLvl")].Clone() as NextLevelButton;
            _startScreenBtn.NextLevelName = data.GetStringProperty("StartScreenName");
            _resumeBtn = prefabs[data.GetStringProperty("ResumeBtnName", "Resume")].Clone() as DisableObjectButton;
            _exitGameBtn = prefabs[data.GetStringProperty("ExitBtnName", "Exit")].Clone() as ExitGameBtn;

            _mainScreenBtnPos = new Vec2(data.GetFloatProperty("MainScreenBtnPosX"), data.GetFloatProperty("MainScreenBtnPosY"));
            _resumeBtnPos = new Vec2(data.GetFloatProperty("ResumeBtnPosX"), data.GetFloatProperty("ResumeBtnPosY"));
            _exitBtnPos = new Vec2(data.GetFloatProperty("ExitBtnPosX"), data.GetFloatProperty("ExitBtnPosY"));

            _openSound = new Sound(data.GetStringProperty("OpenSoundName"));
            _openSoundVolume = data.GetFloatProperty("OpenSoundVolume",1);
            _closeSoundFrequency = data.GetFloatProperty("CloseSoundFrequency", 35000);

            _resumeBtn.OnClick += OnResumeBtnPress;
            
            visible = false;
            //options
            AddChild(new Coroutine(Init()));
        }


        IEnumerator Init()
        {
            yield return null;

            if (GameManager.Instance != null)
                GameManager.Instance.OnWinScreen += OnWinScreen;

        }

        void OnWinScreen()
        {
            if (_opened)
                Toggle();

        }

        void OnResumeBtnPress()
        {
            Toggle();
        }

        void Toggle()
        {
            _opened = !_opened;
            if (_opened)
            {
                AddChild(_startScreenBtn);
                AddChild(_resumeBtn);
                AddChild(_exitGameBtn);
                _resumeBtn.SetPosInVec2(_resumeBtnPos);
                _exitGameBtn.SetPosInVec2 (_exitBtnPos);
                _startScreenBtn.SetPosInVec2(_mainScreenBtnPos);
                _resumeBtn.SetOrigin(_resumeBtn.width / 2,_resumeBtn.height/2);
                _exitGameBtn.SetOrigin(_exitGameBtn.width / 2, _exitGameBtn.height / 2);
                _startScreenBtn.SetOrigin(_startScreenBtn.width / 2, _startScreenBtn.height / 2);


                visible = true;
                GameManager.Instance.MuffleBgMusic();
                _openSound.Play(volume: _openSoundVolume);

            }
            else
            {
                GameManager.Instance.ResetBGMusic();
                RemoveChild(_startScreenBtn);
                RemoveChild(_resumeBtn);
                RemoveChild(_exitGameBtn);
                _resumeBtn.SetOrigin(0, 0);
                _exitGameBtn.SetOrigin(0, 0);
                _startScreenBtn.SetOrigin(0, 0);
                visible = false;
                var chanel = _openSound.Play(volume: _openSoundVolume);
                chanel.Frequency = _closeSoundFrequency;
            }

        }

        void Update()
        {
            if(Input.GetKeyDown(Key.ESCAPE))
            {
                Toggle();
            }
        }

        protected override void OnDestroy()
        {
            _exitGameBtn.OnClick -= OnResumeBtnPress;
            if (GameManager.Instance != null)
                GameManager.Instance.OnWinScreen -= OnWinScreen;
        }
    }
}
