using GXPEngine;
using System.Collections.Generic;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Dragging
{
    public class Dragger : Sprite
    {
        public static Dragger Instance { get; private set; }
        public List<IDraggable> Draggables { get; } = new List<IDraggable>();

        public IDraggable CurrentDrag { get; private set; }
        public bool CanDrag { get; set; } = true;

        public Dragger(TiledObject data) : base("Assets/square.png", true, false)
        {
            if (Instance != null && Instance != this)
            {
                Instance.Destroy();
                Instance = null;
            }
            else
                Instance = this;
        }

        void Update()
        {
            if (!CanDrag) return;

            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);
            if (Input.GetMouseButtonDown(0))
            {
                //search for draggables
                foreach(var draggable in Draggables)
                {
                    if (draggable.ContainsPoint(mousePos))
                    {
                        CurrentDrag = draggable;
                    }
                }
                CurrentDrag?.OnStartDrag(mousePos);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                CurrentDrag?.OnEndDrag(mousePos);
                CurrentDrag = null;
            } 
            else 
            {
                CurrentDrag?.OnStayDrag(mousePos);
            }
        }

        protected override void OnDestroy()
        {
            Instance = null;
        }

    }
}
