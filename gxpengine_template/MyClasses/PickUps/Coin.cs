using System;
using gxpengine_template.MyClasses.PickUps;
using TiledMapParser;

namespace gxpengine_template
{
    public class Coin : PickUp
    {

        public Coin(string fileName, int c, int r, TiledObject data) : base(fileName, c,r,data)
        {

        }

        protected override void Grab(ITrigger taker)
        {
            Console.WriteLine("Increase coin amount");
        }

        void Update()
        {
            AnimateFixed();
        }
        
    }
}
