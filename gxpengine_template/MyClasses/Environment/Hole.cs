using GXPEngine;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Hole : TiledGameObject
    {
        // Sounds
        Sound _winSound;
        float _volume;

        Player player;
        float _persentageOfBall;

        public Hole(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _persentageOfBall = data.GetFloatProperty("PersentageOfBall");
            AddChild(new Coroutine(Init()));

            // Sounds
            _winSound = new Sound(data.GetStringProperty("SoundFileName", "Assets/Sounds/Win sound 1.mp3"));
            _volume = data.GetFloatProperty("Volume");
        }

        IEnumerator Init()
        {
            yield return null;
            player = MyUtils.MyGame.FindObjectOfType<Player>();
            var p = parent;
            parent.RemoveChild(this);
            p.AddChild(this);
        }

        void Update()
        { 
            if (player == null) return;

            float dist = this.GetPosInVec2().DistanceTo(player.GetPosInVec2());
            if (width * .5f > dist + player.width * .5f * _persentageOfBall)
            {
                _winSound.Play(volume:_volume);
                player.Destroy();
                player = null;
                GameManager.Instance.SpawnWinScreen();

            }
        }
    }
}
