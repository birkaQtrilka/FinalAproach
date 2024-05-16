using GXPEngine;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    public class NextLevelButton : Button, IPrefab
    {
        public string NextLevelName { get; set; }
        public NextLevelButton(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, data)
        {
            NextLevelName = data.GetStringProperty("LevelName");
        }

        
        protected NextLevelButton(string filename, int cols, int rows, string lvlName, Sound clickSound, float clickVolume = 1) : base(filename, cols, rows, null, clickSound, clickVolume)
        {
            NextLevelName = lvlName;
        }

        protected override void OnButtonPress()
        {
            MyUtils.MyGame.LoadLevel(NextLevelName);
        }

        public GameObject Clone()
        {
            return new NextLevelButton(texture.filename,_cols, _rows, NextLevelName, clickSound, clickVolume);
        }
    }
}
