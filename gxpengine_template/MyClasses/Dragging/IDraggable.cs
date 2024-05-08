
using GXPEngine;

namespace gxpengine_template.MyClasses.Dragging
{
    public interface IDraggable
    {
        void OnStartDrag(Vec2 mousePos);
        void OnEndDrag(Vec2 mousePos);
        void OnStayDrag(Vec2 mousePos);

        bool ContainsPoint(Vec2 p);
    }
}
