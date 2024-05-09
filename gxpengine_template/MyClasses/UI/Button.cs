using GXPEngine;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public abstract class Button : AnimationSprite
    {
        public Button(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true,false)
        {
            SetOrigin(width / 2f, height / 2f);
        }

        protected void Update()
        {
            if (new Boundary(x, y, width, height).Contains(new Vec2(Input.mouseX, Input.mouseY)))
            {
                SetColor(1, 0, 0);
                if (Input.GetMouseButtonDown(0))
                {
                    OnButtonPress();
                }
            }
            else
                SetColor(0, 1, 0);


        }

        protected abstract void OnButtonPress();
    }
}
