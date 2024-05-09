using gxpengine_template.MyClasses.Dragging;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Hole : TiledGameObject
    {
        Player player;
        string _nextLevelName;

        public Hole(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _nextLevelName = data.GetStringProperty("NextLevelName");
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            player = MyUtils.MyGame.FindObjectOfType<Player>();

        }
        void Update()
        {
            if (player == null) return;

            float dist = this.GetPosInVec2().DistanceTo(player.GetPosInVec2());
            if (width * .5f > dist + player.width * .5f)
            {
                player.Destroy();
                Dragger.Instance.CanDrag = false;
                MyUtils.MyGame.LoadLevel(_nextLevelName);
            }
        }
    }
}
