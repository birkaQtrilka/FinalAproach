using GXPEngine;
using GXPEngine.Core;
using System;
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
                SetColor(1, 0, 0);
                if (Input.GetMouseButtonDown(0))
                {
                    OnButtonPress();
                    OnClick?.Invoke();
                }
            }
            else
                SetColor(0, 1, 0);


        }

        protected abstract void OnButtonPress();
    }
}
