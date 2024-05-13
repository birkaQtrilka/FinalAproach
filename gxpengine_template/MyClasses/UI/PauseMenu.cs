using GXPEngine;
using System.Collections.Generic;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class PauseMenu : TiledGameObject
    {
        readonly NextLevelButton _startScreenBtn;
        readonly DisableObjectButton _resumeBtn;
        readonly ExitGameBtn _exitGameBtn;

        bool _opened;

        public PauseMenu(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            Dictionary<string, IPrefab> prefabs = MyUtils.MyGame.Prefabs;
            _startScreenBtn = prefabs[data.GetStringProperty("GoToMainScreenBtnName", "NxtLvl")].Clone() as NextLevelButton;
            _startScreenBtn.NextLevelName = data.GetStringProperty("StartScreenName");
            _resumeBtn = prefabs[data.GetStringProperty("ResumeBtnName", "Resume")].Clone() as DisableObjectButton;
            _exitGameBtn = prefabs[data.GetStringProperty("ExitBtnName", "Exit")].Clone() as ExitGameBtn;
            _resumeBtn.OnClick += OnResumeBtnPress;
            visible = false;
            //options
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
                _resumeBtn.SetXY(55, 0);
                _exitGameBtn.SetXY(110, 0);
                visible = true;

            }
            else
            {
                RemoveChild(_startScreenBtn);
                RemoveChild(_resumeBtn);
                RemoveChild(_exitGameBtn);
                visible = false;

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
        }
    }
}
