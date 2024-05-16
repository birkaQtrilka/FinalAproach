using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using Physics;
using System;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Booster : Draggable, IPrefab
    {

        readonly TiledObject _data;
        float _boostPower;
        Vec2 _boostDir;

        Sound _boostSound;
        float _boostVolume;
        bool _exitedPlayer;

        public Booster(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _data = data;
            _boostPower = data.GetFloatProperty("BoostPower", 3);
            _boostDir = new Vec2(data.GetFloatProperty("BoostDirX"), data.GetFloatProperty("BoostDirY"));
            _boostSound = new Sound(data.GetStringProperty("BoostSound"));
            _boostVolume = data.GetFloatProperty("BoostVolume", 1);

            _boostDir.Normalize();
            SetOrigin(width / 2, height / 2);
            rotation = data.Rotation;
        }

        void Update()
        {
            var col = trigger?.GetSolidOverlaps().FirstOrDefault(x => x.rbOwner is Mover);

            if (col != null)
            {
                var mover = col.rbOwner as Mover;
                mover.acceleration += _boostDir * _boostPower / mover.Mass ;
                if(!_exitedPlayer)
                {
                    _boostSound.Play(volume: _boostVolume);
                    _exitedPlayer = true;
                }
            }
            else
            {
                _exitedPlayer = false;
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
            clone.rotation = rotation;
            return clone;
        }

    }
}
