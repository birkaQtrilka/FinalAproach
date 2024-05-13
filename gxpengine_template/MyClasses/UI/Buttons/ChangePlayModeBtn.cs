using gxpengine_template.MyClasses.Dragging;
using gxpengine_template.MyClasses.Environment;
using TiledMapParser;
using GXPEngine;
using System.Collections;

namespace gxpengine_template.MyClasses.UI
{
    public class ChangePlayModeBtn : Button
    {
        /*temporary for testing*/
        Vec2 testShoot;
        bool playMode;
        Player player;

        public ChangePlayModeBtn(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            testShoot = new Vec2(data.GetFloatProperty("testX", 40), data.GetFloatProperty("testY",40));
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            player = MyUtils.MyGame.CurrentLevel.FindObjectOfType<Player>();
        }

        protected override void OnButtonPress()
        {
            playMode = !playMode;
            if(playMode)
            {
                player.SetPlayMode();
                Dragger.Instance.CanDrag = false;
                player.Shoot(testShoot);
            }
            else
            {
                player.SetIdleMode();
                player.SetPosInVec2(player.StartPos);
                
                Dragger.Instance.CanDrag = true;

            }
        }
    }
}
