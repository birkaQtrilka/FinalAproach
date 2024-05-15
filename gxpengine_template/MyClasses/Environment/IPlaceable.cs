using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gxpengine_template.MyClasses.Environment
{
    public interface IPlaceable : IPrefab
    {
        event Action<IPlaceable> Placed;
        string MenuImg { get; }
        float MenuImageRotation { get; }
        void Place();

    }
}
