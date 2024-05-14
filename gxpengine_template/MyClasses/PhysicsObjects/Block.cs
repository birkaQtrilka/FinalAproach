using GXPEngine;
using Physics;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.PhysicsObjects
{
    public class Block : TiledGameObject
    {
        public readonly float bounciness;

        public Block(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            this.bounciness = data.GetFloatProperty("Bounciness", .98f);
            AddChild(new Coroutine(Init()));
            alpha = 0;
            SetColor(1, 0, 0);
        }

        public Block(string filename, int cols, int rows, float bounciness) : base(filename, cols, rows, null)
        {
            this.bounciness = bounciness;
            AddChild(new Coroutine(Init()));
            alpha = 0;
            SetColor(1, 0, 0);
        }

        IEnumerator Init()
        {
            yield return null;

            var block = new StaticObj(this, false, bounciness);
            block.SetCollider(new Rectangle(block, this.GetPosInVec2(), new Vec2(width / 2, height / 2)));
            block.Enabled = true;
        }
        
    }
}
