using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template.MyClasses.PickUps
{
    public interface ITrigger
    {
        event Action<GameObject> TriggerEnter;
    }
}
