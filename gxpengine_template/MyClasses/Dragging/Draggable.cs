using GXPEngine;
using gxpengine_template.MyClasses.Environment;
using gxpengine_template.MyClasses.UI;
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
        public event Action<IPlaceable> FailPlace;

        public string MenuImg { get; private set; }
        public float MenuImageRotation { get; private set; }
        public Vec2 OrigPosition { get; set; }
        public bool InInventory { get; set; }
        public Sprite MenuSprite { get; set; }

        protected StaticObj trigger;
        
        bool _wasPlaced;

        // Sound
        Sound _placingItems;
        float _volume;

        public Draggable(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            MenuImg = data.GetStringProperty("MenuImage", "Assets/square.png");
            MenuImageRotation = data.GetFloatProperty("MenuImageRotation", 0);
            AddChild(new AnimationCycler(this, data.GetIntProperty("AnimationDelayMs", 500)));
            visible = false;
            AddChild(new Coroutine(Init()));
            

            _placingItems = new Sound(data.GetStringProperty("SoundFileName", "Assets/Sounds/PlacingItems.wav"));
            _volume = data.GetFloatProperty("Volume");
        }

        IEnumerator Init()
        {
            yield return null;
            trigger = new StaticObj(this, true);
            trigger.SetCollider(new Rectangle(trigger, new Vec2(x, y), new Vec2(width / 2, height / 2)));//make rect collider
            
            Dragger.Instance?.Draggables.Add(this);
            OrigPosition = this.GetPosInVec2();
        }

        public void Place()
        {
            OrigPosition = this.GetPosInVec2();
            _wasPlaced = true;
            Placed?.Invoke(this);
        }
        
        //protected void Update()
        //{
        //    Boundary r;
        //    if (InInventory)
        //    {
        //        var vec = MenuSprite.parent.TransformPoint(MenuSprite.x, MenuSprite.y);
        //         r = new Boundary(vec.x, vec.y, MenuSprite.width, MenuSprite.height);
        //    }
        //    else
        //        r = new Boundary(x, y, width, height);
        //    Gizmos.DrawLine(r.Position.x - r.Size.x / 2, r.Position.y - r.Size.y / 2, r.Position.x - r.Size.x / 2, r.Position.y + r.Size.y / 2);
        //}

        public bool ContainsPoint(Vec2 p)
        {
            if (InInventory)
            {
                var vec = MenuSprite.parent.TransformPoint(MenuSprite.x, MenuSprite.y);
                return new Boundary(vec.x,vec.y,MenuSprite.width,MenuSprite.height).Contains(p);
            }
            return new Boundary(x, y, width, height).Contains(p);
        }

        public void OnEndDrag(Vec2 mousePos)
        {
            bool canPlace = false;
            if (Table.Instance.OnTable(mousePos))
            {
                canPlace = true;

                foreach (Collider col in trigger.GetOverlaps())
                    if (col.rbOwner is IPlaceable || (col.rbOwner is CollisionInteractor rb && !rb.IsTrigger))
                    {
                        canPlace = false;
                        break;
                    }
            }

            if (canPlace)
            {
                Place();
                _placingItems.Play(volume: _volume);
            }
                
            else
            {
                //for inventory UI
                if (!_wasPlaced)
                    visible = false;
                this.SetPosInVec2(OrigPosition);
                trigger.UpdateColliderPosition();
                FailPlace?.Invoke(this);

            }
        }

        public virtual void OnStartDrag(Vec2 mousePos)
        {
            visible = true;
        }

        public void OnStayDrag(Vec2 mousePos)
        {
            this.SetPosInVec2(mousePos);
            trigger.UpdateColliderPosition();
        }

        protected override void OnDestroy()
        {
            Dragger.Instance?.Draggables.Remove(this);
            Placed = null;
            FailPlace = null;
        }

        public abstract GameObject Clone();
    }
}
