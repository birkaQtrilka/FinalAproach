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

        public Button(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            SetOrigin(width / 2f, height / 2f);
        }

        protected void Update()
        {
            if (parent == null) return;
            Vector2 worldPos = parent.TransformPoint(x, y);
            if (new Boundary(worldPos.x, worldPos.y, width, height).Contains(new Vec2(Input.mouseX, Input.mouseY)))
            {
                //hover state
                if (Input.GetMouseButtonDown(0))
                {
                    //before state
                    AddChild(new Coroutine(SlightPauseAfterClick()));
                    
                }
            }
            //else
            //    not hover state


        }

        IEnumerator SlightPauseAfterClick()
        {
            yield return new WaitForSeconds(0.06f);
            OnButtonPress();
            OnClick?.Invoke();
        }

        protected abstract void OnButtonPress();
    }
}
