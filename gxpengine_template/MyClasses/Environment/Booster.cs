using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using Physics;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Booster : Draggable, IPrefab
    {

        readonly TiledObject _data;
        float _boostPower;
        public Booster(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _data = data;
            _boostPower = data.GetFloatProperty("BoostPower", 3);
        }

        void Update()
        {
            var col = trigger?.GetSolidOverlaps().FirstOrDefault(x => x.owner is Mover);

            if (col != null)
            {
                var mover = col.owner as Mover;
                //if (mover.velocity.Length < 5)
                    mover.acceleration += -Vec2.right * _boostPower / mover.Mass ;
            }
        }

        public override GameObject Clone()
        {
            var clone = new Booster(texture.filename, _cols, _rows, _data);
            clone.name = name;
            parent?.AddChild(clone);
            clone.SetPosInVec2(this.GetPosInVec2());
            clone.width = width;
            clone.height = height;
            clone.SetOrigin(width/2,height/2);
            return clone;
        }

    }
}
