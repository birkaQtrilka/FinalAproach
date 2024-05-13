using Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Fan : TiledGameObject
    {
        StaticObj _trigger;
        public Fan(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            alpha = .3f;
            AddChild(new Coroutine(Init()));   
        }

        IEnumerator Init()
        {
            yield return null;
            _trigger = new StaticObj(this);
            _trigger.SetCollider(new Rectangle(_trigger, this.GetPosInVec2(), width / 2));
            _trigger.OnTriggerStay += OnTriggerStay;
        }

        bool t;
        private void OnTriggerStay(Collider obj)
        {
            t = true;
        }

        void Update()
        {
             //if( _trigger != null ) 
          //  (_trigger.Collider as Rectangle).DrawSelf();

            if(t)
            {
                SetColor(1, 0, 0);
                t = false;
            }
            else
            {
                SetColor(0, 1, 0);
            }
        }
    }
}
