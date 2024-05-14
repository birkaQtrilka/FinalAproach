using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.UI;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Hole : TiledGameObject
    {
        Player player;
        float _persentageOfBall;
        public Hole(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _persentageOfBall = data.GetFloatProperty("PersentageOfBall");
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
            if (width * .5f > dist + player.width * .5f * _persentageOfBall)
            {
                player.Destroy();
                player = null;
                GameManager.Instance.SpawnWinScreen();

            }
        }
    }
}
