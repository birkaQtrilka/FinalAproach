using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private NextLevelButton(string filename, int cols, int rows, string lvlName) : base(filename, cols, rows, null)
        {
            NextLevelName = lvlName;
        }

        protected override void OnButtonPress()
        {
            MyUtils.MyGame.LoadLevel(NextLevelName);
        }

        public GameObject Clone()
        {
            return new NextLevelButton(texture.filename,_cols, _rows, NextLevelName);
        }
    }
}
