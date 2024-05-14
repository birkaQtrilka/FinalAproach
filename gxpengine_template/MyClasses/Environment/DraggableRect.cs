using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.PhysicsObjects;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class DraggableRect : Draggable, IPrefab
    {
        Block block;
        readonly TiledObject _data;

        public DraggableRect(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
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
        }
        public override void OnStartDrag(Vec2 mousePos)
        {
            base.OnStartDrag(mousePos);
            block.Destroy();

        }

        private void OnPlace(IPlaceable obj)
        {

            UpdateRamp();
        }

        void UpdateRamp()
        {
            block = new Block(texture.filename, 1, 1, _data.GetFloatProperty("Bounciness", .98f),width,height);
            block.alpha = 0;
            MyUtils.MyGame.CurrentLevel.AddChild(block);
            block.SetOrigin(block.width / 2, block.height / 2);
            block.SetXY(x, y);

        }

        public override GameObject Clone()
        {
            var clone = new DraggableRect(texture.filename, _cols, _rows, _data);
            clone.SetOrigin(clone.width / 2, clone.width / 2);
            name = _data.Name;
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

