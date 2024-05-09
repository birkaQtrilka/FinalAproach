using GXPEngine;
using gxpengine_template.MyClasses.Environment;
using Physics;
using System;
using System.Collections;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Dragging
{
    public abstract class Draggable : TiledGameObject, IDraggable, IPlaceable
    {

        public event Action<IPlaceable> Placed;
        public string MenuImg { get; private set; }
        public Vec2 OrigPosition { get; set; }

        protected StaticObj trigger;
        
        bool _wasPlaced;

        public Draggable(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            MenuImg = data.GetStringProperty("MenuImage", "Assets/square.png");

            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            trigger = new StaticObj(this, true);
            trigger.SetCollider(new Circle(this, new Vec2(x, y), width / 2));//make rect collider

            Dragger.Instance?.Draggables.Add(this);
            OrigPosition = this.GetPosInVec2();
        }

        public void Place()
        {
            OrigPosition = this.GetPosInVec2();
            _wasPlaced = true;
            Placed?.Invoke(this);
        }

        public bool ContainsPoint(Vec2 p)
        {
            return new Boundary(x, y, width, height).Contains(p);
        }

        public void OnEndDrag(Vec2 mousePos)
        {
            bool canPlace = false;
            if (Table.Instance.OnTable(mousePos))
            {
                canPlace = true;

                foreach (Collider col in trigger.GetOverlaps())
                    if (col.owner is IPlaceable || (col.owner is CollisionInteractor rb && !rb.IsTrigger))
                    {
                        canPlace = false;
                        break;
                    }
            }

            if (canPlace)
                Place();
            else
            {
                //for inventory UI
                if (!_wasPlaced)
                    visible = false;
                this.SetPosInVec2(OrigPosition);
                trigger.UpdateColliderPosition(x, y);

            }
        }

        public void OnStartDrag(Vec2 mousePos)
        {
            visible = true;
        }

        public void OnStayDrag(Vec2 mousePos)
        {
            this.SetPosInVec2(mousePos);
            trigger.UpdateColliderPosition(x, y);
        }

        protected override void OnDestroy()
        {
            Dragger.Instance?.Draggables.Remove(this);
            Placed = null;
        }

        public abstract GameObject Clone();
    }
}
