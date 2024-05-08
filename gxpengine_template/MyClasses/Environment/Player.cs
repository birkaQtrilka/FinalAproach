using GXPEngine;
using gxpengine_template.MyClasses.TankGame;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Player : AnimationSprite
    {
        readonly Table _table;
        MovingBall _rigidBody;

        public Player(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true, false)
        {
            _table = FindObjectOfType<Table>();
            AddChild(new Coroutine(Start(data)));
        }

        IEnumerator Start(TiledObject data)
        {
            yield return null;
            float vx = data.GetFloatProperty("StartX", 1);
            float vy = data.GetFloatProperty("StartY", 1);
            _rigidBody = new MovingBall(this, new Vec2(vx,vy), new Vec2(x, y), width / 2);

        }

        void Update()
        {
            //if(_table.OnTable(new Vec2(x,y)))
            //{
            //    Destroy();
            //}

        }


    }
}
