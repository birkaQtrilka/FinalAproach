using gxpengine_template.MyClasses.Tweens;
using System;
using System.Collections;
using System.Collections.Generic;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class NextLevelUI : TiledGameObject
    {
        ResetSceneBtn _resetSceneBtn;
        NextLevelButton _nextLevelButton;
        MenuStarsUI _menuStarsUI;
        StarsUI _starsUI;
        public NextLevelUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            Dictionary<string, IPrefab> prefabs = MyUtils.MyGame.Prefabs;
            _resetSceneBtn = prefabs[data.GetStringProperty("ResetBtnName", "Reset")].Clone() as ResetSceneBtn;
            _nextLevelButton = prefabs[data.GetStringProperty("NextLvlBtnName", "NxtLvl")].Clone() as NextLevelButton;
            _nextLevelButton.NextLevelName = data.GetStringProperty("NextLevelName");
            _menuStarsUI = prefabs[data.GetStringProperty("StarsUIName", "StarsUIMenu")].Clone() as MenuStarsUI;

            //completion time?
            
            visible = false;
            _nextLevelButton.SetXY(50, 50);
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            scale = 0;
            _starsUI = MyGame.main.FindObjectOfType<StarsUI>();
        }

        public void Pop()
        {
            AddChild(_resetSceneBtn);
            AddChild(_nextLevelButton);
            AddChild(_menuStarsUI);
            visible = true;
            _menuStarsUI.Score = _starsUI.Score;

            AddChild(new Tween(TweenProperty.scale, 500, 1, EaseFunc.Factory("EaseOutSin")).OnCompleted(()=> _menuStarsUI.PlayAnim()));
        }

        
    }
}
