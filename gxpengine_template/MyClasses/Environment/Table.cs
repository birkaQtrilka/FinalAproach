using GXPEngine;
using TiledMapParser;
using Physics;
using System.Collections;
namespace gxpengine_template.MyClasses.Environment
{
    public class Table : AnimationSprite
    {
        public static Table Instance { get; private set; }


        Boundary _boundary;

        public Table(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1, true, false)
        {
            if(Instance != null && Instance == this)
            {
                Destroy();
                return;
            }
            else
                Instance = this;
            SetOrigin(width / 2, height / 2) ;
            AddChild(new Coroutine(Start()));
        }

        IEnumerator Start()
        {
            yield return null;
            _boundary = new Boundary(new Vec2(width, height), new Vec2(x, y));

        }

        public bool OnTable(Vec2 point)
        {
            return _boundary.Contains(point);
        }

        protected override void OnDestroy()
        {
            Instance = null;
        }
    }
}
