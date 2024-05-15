using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using System.Collections;
using System.Configuration;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class PlayerLauncher : TiledGameObject
    {
        public bool CanLaunch { get; set; } = true;
        Player _player;
        Vec2 _shootVelocity;
        bool _mousePressed;
        bool _canDrag;
        Vec2 _visualVelocity;

        readonly Sprite[] _powerLevels;
        readonly Pivot _container;
        readonly float _speedCap;
        readonly float _visualCap;
        readonly float _spacing;

        public PlayerLauncher(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _speedCap = data.GetFloatProperty("SpeedCap");
            _visualCap = data.GetFloatProperty("VisualCap", 10);
            _spacing = data.GetFloatProperty("ArrowSpacing");
            _powerLevels = data.GetStringProperty("ArrowsSpritesCSV").Split(',').Select(x => new Sprite(x,true,false)).ToArray();
            _container = new Pivot();
            MyGame.main.AddChild(_container);
            


            visible = false;
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;
            _player = MyGame.main.FindObjectOfType<Player>();

            for (int i = 0; i < _powerLevels.Length; i++)
            {
                Sprite arrow = _powerLevels[i];
                arrow.SetOrigin(arrow.width / 2, arrow.height / 2);
                arrow.rotation = 180;

                _container.AddChild(arrow);
                arrow.SetXY(0, i * (arrow.height + _spacing) + arrow.height / 2 + _spacing + _player.width / 2);
            }
        }

        void Update()
        {
            if (_player == null || !CanLaunch || Dragger.Instance.CurrentDrag != null) return;

            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);
            _player.SetColor(1, 1, 1);

            _container.visible = false;
            if (Input.GetMouseButton(0))
            {

                if (!_mousePressed)
                {
                    _player.RigidBody.UpdateColliderPosition();
                    _canDrag = _player.RigidBody.Collider.ContainsPoint(mousePos);
                    _mousePressed = true;
                    //animation; color it?
                }

                if (_canDrag)
                {
                    _visualVelocity = _player.GetPosInVec2() - mousePos;
                    float releaseLength = _visualVelocity.Length;
                    float t = releaseLength / _visualCap;
                    t  = Mathf.Min(t, 1f);
                    Vec2 dir = _visualVelocity.Normalized();
                    _visualVelocity = t * _visualCap * dir;
                    _shootVelocity = t * _speedCap * dir;
                    _player.SetColor(1, 1, 0);
                    
                    DrawArrows(_visualVelocity, t);
                }
            }
            else
            {
                _mousePressed = false;
            }

            if (Input.GetMouseButtonUp(0) && _canDrag)
            {

                GameManager.Instance.StartPlayMode();
                _player.Shoot(_shootVelocity);
                _canDrag = false;
            }

        }

        void DrawArrows(Vec2 powerVector, float t)
        {
            _container.visible = true;
            _container.SetXY(_player.x, _player.y);
            _container.rotation = powerVector.GetAngleDegrees() - 90;

            int i = 0;
            float persentPerVisual = 1f / _powerLevels.Length;
            foreach (var arrow in _powerLevels)
            {
                arrow.visible = i * persentPerVisual < t;
                i++;
            }
        }
    }
}
