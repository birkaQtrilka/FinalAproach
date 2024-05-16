using GXPEngine;
using gxpengine_template.MyClasses.UI;
using System.Collections;
using System.Runtime.InteropServices;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class TutorialShower : TiledGameObject
    {
        readonly Sprite[][] _pages;
        readonly DisableObjectButton _nextPageBtn;
        readonly NextLevelButton _startPlayingBtn;
        readonly float _restTime;
        readonly float _lastFrameRestTime;
        readonly Pivot _bgContainer;

        Vec2 _nextBtnPos;
        Vec2 _startBtnPos;

        Coroutine _currAnimation;

        int currPage;

        public TutorialShower(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            _bgContainer = new Pivot();
            AddChild(_bgContainer );
            _bgContainer.SetXY(-width / 2, -height / 2);
            _restTime = data.GetFloatProperty("RestTimeS", 1);
            _lastFrameRestTime = data.GetFloatProperty("LastFrameRestTimeS", 2);
            string[] animatedPages = data.GetStringProperty("AnimatedPagesCSV").Split(',');
            _pages = new Sprite[animatedPages.Length][];

            _nextBtnPos = new Vec2(data.GetFloatProperty("NextBtnPosX"), data.GetFloatProperty("NextBtnPosY"));
            _startBtnPos = new Vec2(data.GetFloatProperty("StartBtnPosX"), data.GetFloatProperty("StartBtnPosY"));

            _nextPageBtn = MyUtils.MyGame.Prefabs[data.GetStringProperty("NextBtnPrefabName", "NextBtn")].Clone() as DisableObjectButton;
            _nextPageBtn.OnClick += AccesNextPage;
            _nextPageBtn.SetOrigin(_nextPageBtn.width / 2, _nextPageBtn.height / 2);
            AddChild(_nextPageBtn);
            _nextPageBtn.SetPosInVec2(_nextBtnPos);

            _startPlayingBtn = MyUtils.MyGame.Prefabs[data.GetStringProperty("StartBtnPrefabName", "StartBtn")].Clone() as NextLevelButton;
            _startPlayingBtn.NextLevelName = data.GetStringProperty("NextLevelName");
            _startPlayingBtn.SetOrigin(_startPlayingBtn.width / 2, _startPlayingBtn.height / 2);

            



            for (int i = 0; i < animatedPages.Length; i++)
            {
                string[] framesTxt = animatedPages[i].Split(':');
                _pages[i] = new Sprite[framesTxt.Length];
                Sprite[] frames = _pages[i];

                for (int j = 0; j < frames.Length; j++)
                {
                    frames[j] = new Sprite(framesTxt[j], false, false);
                }
            }

            AccesNextPage();
        }

        void AccesNextPage()
        {
            foreach (var frame in _bgContainer.GetChildren())
                _bgContainer.RemoveChild(frame);

            _currAnimation?.Destroy();
            _currAnimation = new Coroutine(AnimatePage(currPage));
            AddChild(_currAnimation);
            currPage++;

            if (currPage == _pages.Length)
            {
                RemoveChild(_nextPageBtn);
                AddChild(_startPlayingBtn);
                _startPlayingBtn.SetPosInVec2(_startBtnPos);
            }
        }

        IEnumerator AnimatePage(int index)
        {
            Sprite[] page = _pages[index];

            int currFrame = 0;
            int maxFrames = page.Length;
            Sprite prevFrame = null;
            while(true)
            {
                if (prevFrame != null)
                    _bgContainer.RemoveChild(prevFrame);

                prevFrame = page[currFrame];
                _bgContainer.AddChild(prevFrame);

                currFrame++;
                if (currFrame >= maxFrames)
                {
                    currFrame = 0;
                    yield return new WaitForSeconds(_lastFrameRestTime);
                }
                else
                    yield return new WaitForSeconds(_restTime);

            }

            
        }

        protected override void OnDestroy()
        {
            _nextPageBtn.OnClick -= AccesNextPage;

        }
    }
}
