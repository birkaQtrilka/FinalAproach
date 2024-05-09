using GXPEngine;
using Physics;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.PhysicsObjects
{
    public class Ramp : TiledGameObject
    {
        public readonly float bounciness;

        Vec2 start1;
        Vec2 end1;
        Vec2 end2;

        public Ramp(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            this.bounciness = data.GetFloatProperty("Bounciness", .98f);
            AddChild(new Coroutine(Init()));
            alpha = 0;
            SetColor(1, 0, 0);
        }

        public Ramp(string filename, int cols, int rows, float bounciness) : base(filename, cols, rows, null)
        {
            this.bounciness = bounciness;
            AddChild(new Coroutine(Init()));
            alpha = 0;
            SetColor(1, 0, 0);
        }

        IEnumerator Init()
        {
            yield return null;
            Vec2 pos = this.GetPosInVec2();
            float rads = Vec2.Deg2Rad(rotation);
            
            var dir = new Vec2(-width/2, height/2);
            dir.RotateRadians(rads);
            start1 = dir + pos;

            dir = new Vec2(-width / 2, -height / 2);
            dir.RotateRadians(rads);
            end1 = dir + pos;
            
            dir = new Vec2(width / 2, -height / 2);
            dir.RotateRadians(rads);
            end2 = dir + pos;

            var edge1 = new StaticObj(this, false, bounciness);
            var edge2 = new StaticObj(this, false, bounciness);
            var edge3 = new StaticObj(this, false, bounciness);

            var cap1 = new StaticObj(this, false, bounciness);
            var cap2 = new StaticObj(this, false, bounciness);
            var cap3 = new StaticObj(this, false, bounciness);

            edge1.SetCollider(new AngledLine(edge1, end1, start1));
            edge2.SetCollider(new AngledLine(edge2, end2, end1));
            edge3.SetCollider(new AngledLine(edge3, start1, end2));

            cap1.SetCollider(new Circle(cap1, start1, 0));
            cap2.SetCollider(new Circle(cap2, end1, 0));
            cap3.SetCollider(new Circle(cap3, end2, 0));
        }

        //void Update()
        //{
        //    Gizmos.DrawLine(start1.x, start1.y, end1.x, end1.y);
        //    Gizmos.DrawLine(end1.x, end1.y, end2.x, end2.y);
        //    Gizmos.DrawLine(start1.x, start1.y, end2.x, end2.y);

        //}

    }
}
