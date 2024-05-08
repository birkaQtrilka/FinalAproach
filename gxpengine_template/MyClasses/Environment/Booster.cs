using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using Physics;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Booster : AnimationSprite, IDraggable, IPlaceable
    {
        StaticObj _trigger;
        bool _canDrag ;
        Vec2 _origPosition;

        public Booster(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1, true, false)
        {
            _trigger = new StaticObj(this, true);

            _trigger.SetCollider(new Circle(this, new Vec2(data.X, data.Y), width / 2));
            _origPosition.SetXY(data.X, data.Y);
            Dragger.Instance.Draggables.Add(this);
        }

        public void Place()
        {
            _canDrag = false;
            
        }

        public bool ContainsPoint(Vec2 p)
        {
            return new Boundary(x,y,width,height).Contains(p);
        }

        public void OnEndDrag(Vec2 mousePos)
        {
            bool canPlace = false;
            Table table;
            foreach (Collider col in _trigger.GetTriggerOverlaps())
            {
                if(col.owner is Table t)
                {
                    canPlace = true;
                    table = t;
                }
                else if(col.owner is IPlaceable)
                {
                    canPlace = false;
                    break;
                }
            }

            if(canPlace)
            {
                Place();
            }
            else
            {
                this.SetVec(_origPosition);
                _trigger.UpdateColliderPosition(x, y);

            }
        }

        public void OnStartDrag(Vec2 mousePos)
        {
            _canDrag = true;
        }

        public void OnStayDrag(Vec2 mousePos)
        {

            if(_canDrag)
            {
                this.SetVec(mousePos);
                _trigger.UpdateColliderPosition(x,y);

            }
        }

        protected override void OnDestroy()
        {
            Dragger.Instance.Draggables.Remove(this);

        }
    }
}
