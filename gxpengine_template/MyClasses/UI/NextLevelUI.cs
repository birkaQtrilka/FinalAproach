using GXPEngine;
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
        Sprite _youWinText;

        Vec2 _resetBtnPos;
        Vec2 _nextLvlBtnPos;
        Vec2 _menuStarsPos;
        Vec2 _youWinPos;
        float _bgVolume;

        public NextLevelUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            Dictionary<string, IPrefab> prefabs = MyUtils.MyGame.Prefabs;
            _resetSceneBtn = prefabs[data.GetStringProperty("ResetBtnName", "Reset")].Clone() as ResetSceneBtn;
            _resetSceneBtn.SetOrigin(_resetSceneBtn.width/2,_resetSceneBtn.height/2);

            _nextLevelButton = prefabs[data.GetStringProperty("NextLvlBtnName", "NxtLvl")].Clone() as NextLevelButton;
            _nextLevelButton.SetOrigin(_nextLevelButton.width / 2, _nextLevelButton.height / 2);
            _nextLevelButton.NextLevelName = data.GetStringProperty("NextLevelName");

            _menuStarsUI = prefabs[data.GetStringProperty("StarsUIName", "StarsUIMenu")].Clone() as MenuStarsUI;
            _menuStarsUI.SetOrigin(_menuStarsUI.width / 2, _resetSceneBtn.height / 2);
            
            _youWinText = new Sprite(data.GetStringProperty("YouWinTextName"),true,false);
            _youWinText.SetCenterOrigin();

            _menuStarsPos = new Vec2(data.GetFloatProperty("MenuStarsPosX"), data.GetFloatProperty("MenuStarsPosY"));
            _nextLvlBtnPos = new Vec2(data.GetFloatProperty("NextLvlBtnPosX"), data.GetFloatProperty("NextLvlBtnPosY"));
            _resetBtnPos = new Vec2(data.GetFloatProperty("ResetBtnPosX"), data.GetFloatProperty("ResetBtnPosY"));
            _youWinPos = new Vec2(data.GetFloatProperty("YouWinTextPosX"), data.GetFloatProperty("YouWinTextPosY"));
            //completion time?
            _bgVolume = data.GetFloatProperty("BgVolume",0.1f);
            visible = false;
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
            AddChild(_youWinText);

            _resetSceneBtn.SetPosInVec2(_resetBtnPos);
            _nextLevelButton.SetPosInVec2(_nextLvlBtnPos);
            _menuStarsUI.SetPosInVec2 (_menuStarsPos);
            _youWinText.SetPosInVec2(_youWinPos);

            visible = true;
            _menuStarsUI.Score = _starsUI.Score;

            GameManager.Instance.BgMusic.Volume = _bgVolume;
            //GameManager.Instance.BgMusic.Stop();
            AddChild(new Tween(TweenProperty.scale, 500, 1, EaseFunc.Factory("EaseOutSin")).OnCompleted(()=> _menuStarsUI.PlayAnim()));
        }

        
    }
}
