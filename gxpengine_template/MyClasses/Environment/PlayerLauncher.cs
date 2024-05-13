using GXPEngine;
using gxpengine_template.MyClasses.Dragging;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class PlayerLauncher : TiledGameObject
    {
        public bool CanLaunch { get; set; } = true;
        Player _player;
        readonly float _speedCap;

        public PlayerLauncher(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _speedCap = data.GetFloatProperty("SpeedCap");
            visible = false;
            AddChild(new Coroutine(Init()));
        }

        IEnumerator Init()
        {
            yield return null;

            _player = MyGame.main.FindObjectOfType<Player>();
        }
        
        bool _mousePressed;
        bool _canDrag;
        Vec2 _releaseVelocity;

        void Update()
        {
            if (_player == null || !CanLaunch) return;

            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);
            _player.SetColor(1, 1, 1);


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
                    _releaseVelocity = _player.GetPosInVec2() - mousePos;
                    if (_releaseVelocity.Length > _speedCap)
                    {
                        _releaseVelocity = _releaseVelocity.Normalized() * _speedCap;
                    }
                    _player.SetColor(1, 1, 0);
                    Gizmos.DrawArrow(_player.x, _player.y, _releaseVelocity.x, _releaseVelocity.y);
                }
            }
            else
            {
                _mousePressed = false;
            }

            if (Input.GetMouseButtonUp(0) && _canDrag)
            {
                //start playMode
                //put this in GameManager?

                _player.SetPlayMode();
                Dragger.Instance.CanDrag = false;
                _player.Shoot(_releaseVelocity);
                CanLaunch = false;
                _canDrag = false;
            }

        }
    }
}
