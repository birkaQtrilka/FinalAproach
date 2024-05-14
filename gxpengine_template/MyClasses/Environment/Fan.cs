using GXPEngine;
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
        float _windPower;

        public Fan(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            alpha = .3f;
            _windPower = data.GetFloatProperty("WindPower");
            AddChild(new Coroutine(Init()));   
        }

        IEnumerator Init()
        {
            yield return null;
            _trigger = new StaticObj(this, false);
            _trigger.SetCollider(new Rectangle(_trigger, this.GetPosInVec2(), new Vec2(width/2,height/2)));
            _trigger.Enabled = true;
            _trigger.OnTriggerStay += OnTriggerStay;
        }

        private void OnTriggerStay(Collider col)
        {
            if (col.rbOwner.parent is Player player)
            {
                player.RigidBody.acceleration = -Vec2.right * _windPower;
            }
        }

        protected override void OnDestroy()
        {
            _trigger.OnTriggerStay -= OnTriggerStay;

        }
    }
}
