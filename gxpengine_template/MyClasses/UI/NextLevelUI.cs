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
        MenuStarsUI _starsUI;

        public NextLevelUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            Dictionary<string, IPrefab> prefabs = MyUtils.MyGame.Prefabs;
            _resetSceneBtn = prefabs[data.GetStringProperty("ResetBtnName", "Reset")].Clone() as ResetSceneBtn;
            _nextLevelButton = prefabs[data.GetStringProperty("NextLvlBtnName", "NxtLvl")].Clone() as NextLevelButton;
            _nextLevelButton.NextLevelName = data.GetStringProperty("NextLevelName");
            _starsUI = prefabs[data.GetStringProperty("StarsUIName", "StarsUIMenu")].Clone() as MenuStarsUI;

            //completion time?
            
            visible = false;
            _nextLevelButton.SetXY(50, 50);
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            scale = 0;

        }

        public void Pop()
        {
            AddChild(_resetSceneBtn);
            AddChild(_nextLevelButton);
            AddChild(_starsUI);
            visible = true;
            _starsUI.Score = StarsUI.Score;

            AddChild(new Tween(TweenProperty.scale, 500, 1, EaseFunc.Factory("EaseOutSin")).OnCompleted(()=> _starsUI.PlayAnim()));
        }

        
    }
}
