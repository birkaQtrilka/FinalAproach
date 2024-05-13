using GXPEngine;
using Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template.MyClasses.PhysicsObjects
{
    public class TriggerBox : StaticObj
    {
        public TriggerBox(GameObject owner, float radius, bool isTrigger = false, float bounciness = 1) : base(owner, isTrigger, bounciness)
        {
            SetCollider(new Rectangle(this, parent.GetPosInVec2(), radius));
        }
    }
}
