using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public abstract class Button : TiledGameObject
    {
        public event Action OnClick;
        protected Sound clickSound;
        protected float clickVolume;
        protected float buttonPause;
        bool _clicking;

        public Button(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            if (data != null)
                clickSound = new Sound(data.GetStringProperty("ClickSoundName", "Assets/Sounds/PlacingItems.wav"));

            clickVolume = data.GetFloatProperty("ClickSoundVolume", 1);
            buttonPause = data.GetFloatProperty("ButtonPauseBeforeClick", 0.1f);
            SetOrigin(width / 2f, height / 2f);
        }

        protected Button(string filename, int cols, int rows, TiledObject data, Sound clickSound, float clickVolume = 1, float buttonPause = 0.1f) : base(filename, cols, rows, data)
        {
            this.buttonPause = buttonPause;
            this.clickSound = clickSound ?? new Sound("Assets/Sounds/PlacingItems.wav");
            this.clickVolume = clickVolume;
        }

        protected void Update()
        {
            if (parent == null) return;
            SetColor(1, 1, 1);

            Vector2 worldPos = parent.TransformPoint(x, y);
            if (new Boundary(worldPos.x, worldPos.y, width, height).Contains(new Vec2(Input.mouseX, Input.mouseY)))
            {
                //hover state
                if(!_clicking)
                    SetFrame(1);

                if (Input.GetMouseButtonDown(0))
                {
                    //before state
                    SetFrame(2);
                    _clicking = true;
                    clickSound.Play(volume: clickVolume);
                    AddChild(new Coroutine(SlightPauseAfterClick()));
                    
                }
            }
            else if (currentFrame != 0 && !_clicking)
                SetFrame(0);


        }

        IEnumerator SlightPauseAfterClick()
        {
            yield return new WaitForSeconds(buttonPause);
            _clicking = false;
            OnButtonPress();
            OnClick?.Invoke();
        }

        protected abstract void OnButtonPress();
    }
}
