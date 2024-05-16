using GXPEngine;
using Physics;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.PhysicsObjects
{
    public class Block : TiledGameObject
    {
        public readonly float bounciness;
        
        StaticObj _block;

        public Block(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            bounciness = data.GetFloatProperty("Bounciness", .98f);
            AddChild(new AnimationCycler(this, 500));

            AddChild(new Coroutine(Init()));
        }

        public Block(string filename, int cols, int rows, float bounciness, int w, int h) : base(filename, cols, rows, null)
        {
            this.bounciness = bounciness;
            width = w; 
            height = h;  
            AddChild(new Coroutine(Init()));
        }
        
        IEnumerator Init()
        {
            yield return null;

            _block = new StaticObj(this, false, bounciness);
            _block.SetCollider(new Rectangle(_block, this.GetPosInVec2(), new Vec2(width / 2, height / 2)));
            _block.Enabled = true;
        }

    }
}
