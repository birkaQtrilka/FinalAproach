using GXPEngine;
using TiledMapParser;

namespace gxpengine_template.MyClasses
{
    public class TiledGameObject : AnimationSprite
    {
        public TiledGameObject(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true,false)
        {
        }
    }
}
