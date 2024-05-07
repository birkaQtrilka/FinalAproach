using GXPEngine;
using Physics;
using TiledMapParser;

namespace gxpengine_template.MyClasses
{
    public class Edge : AnimationSprite
    {
        Vec2 _start;
        Vec2 _end;

        public Edge(string fileName, int c, int r, TiledObject data) : base(fileName,c,r,-1,true,false)
        {
            var bouncines = data.GetFloatProperty("Bounciness", .98f);
            var pos = new Vec2(data.Width, -data.Height);
            pos.RotateDegrees(data.Rotation);

            _start = new Vec2(data.X, data.Y);
            _end = new Vec2(data.X, data.Y) + pos;


            var edge1 = new StaticObj(this, false, bouncines);
            var edge2 = new StaticObj(this, false, bouncines);
            var cap1 = new StaticObj(this, false, bouncines);
            var cap2 = new StaticObj(this, false, bouncines);

            edge1.SetCollider(new AngledLine(edge1, _start, _end));
            edge2.SetCollider(new AngledLine(edge2, _end, _start));
            cap1.SetCollider(new Circle(cap1, _start, 0));
            cap2.SetCollider(new Circle(cap2, _end, 0));

            alpha = 0f;
        }

        void Update()
        {
            Gizmos.DrawLine(_start.x, _start.y, _end.x, _end.y);
        }

    }
}
