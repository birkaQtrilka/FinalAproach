using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.PhysicsObjects;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class DraggableRamp : Draggable, IPrefab
    {
        Ramp ramp;
        readonly TiledObject _data;

        public DraggableRamp(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _data = data;
            name = _data.Name;
            Placed += OnPlace;
            FailPlace += OnFailPlaced; 
            AddChild(new Coroutine(Init()));
        }

        void OnFailPlaced(IPlaceable obj)
        {
            UpdateRamp();

        }

        IEnumerator Init()
        {
            yield return null;
            UpdateRamp();
            yield return new WaitForSeconds(0.2f);
            ramp.DisableColliders();
        }

        public override void OnStartDrag(Vec2 mousePos)
        {
            base.OnStartDrag(mousePos);
            ramp.Destroy();

        }

        void OnPlace(IPlaceable obj)
        {

            UpdateRamp();
        }
        
        void UpdateRamp()
        {
            ramp = new Ramp("Assets/ramp.png", 1, 1, _data.GetFloatProperty("Bounciness", .98f));
            ramp.alpha = 0;
            MyUtils.MyGame.CurrentLevel.AddChild(ramp);
            ramp.SetOrigin(ramp.width / 2, ramp.height / 2);
            ramp.rotation = rotation;
            ramp.SetXY(x, y );

            ramp.rotation = rotation;
        }

        public override GameObject Clone()
        {
            var clone = new DraggableRamp(texture.filename,_cols,_rows, _data);
            clone.SetOrigin(clone.width / 2, clone.width / 2);
            name = _data.Name;
            clone.rotation = rotation;
            return clone;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Placed -= OnPlace;
            FailPlace -= OnFailPlaced;

        }
    }
}
