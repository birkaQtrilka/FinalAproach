using GXPEngine;

namespace gxpengine_template.MyClasses
{
    //center pivot point
    public struct Boundary
    {
        public Vec2 Size;
        public Vec2 Position;

        public Boundary(Vec2 size, Vec2 position)
        {
            Size = size;
            Position = position;
        }
        public Boundary(float x, float y, float w, float h)
        {
            Size = new Vec2(w,h);
            Position = new Vec2(x,y);
        }

        public bool Contains(Vec2 point)
        {
            return point.x >= Position.x - Size.x * .5f && point.x <= Position.x + Size.x * .5f
                && point.y >= Position.y - Size.y * .5f && point.y <= Position.y + Size.y * .5f;
        }
    }
}
