using System;
using System.Linq;
using GXPEngine;
using gxpengine_template.MyClasses.SceneHandling;
using TiledMapParser;

namespace gxpengine_template
{
    public class Level : GameObject
    {
        
        public event Action<Level> LevelStarted;

        public Level(string fileName) 
        {
            name = fileName;
        }

        public void Init()
        {
            
            var loader = new TiledLoader(name, MyGame.main, addColliders: false, autoInstance: true);
            int index;
            SceneConfigs sceneConfigs = null;

            //sceneConfigs
            if (loader.map.ObjectGroups.TryGetIndex(x => x.Name == "SceneConfig", out index))
            {
                loader.rootObject = MyGame.main;
                loader.LoadObjectGroups(index);
                sceneConfigs = game.FindObjectOfType<SceneConfigs>();
                sceneConfigs.Init(this);
            }

            foreach (LayerConfig layer in sceneConfigs.ObjectLayers)
            {
                if (loader.map.ObjectGroups.TryGetIndex(x => x.Name == layer.LayerName, out index))
                {
                    loader.rootObject = MyGame.main.FindChildByName(layer.ParentName);
                    loader.LoadObjectGroups(index);
                }
            }
            //maybe start after frame with coroutine?
            LevelStarted?.Invoke(this);

            //it's useless after Scene Load
            sceneConfigs?.LateDestroy();
        }
        

        #region Scrolling
        //void Update()
        //{
        //    Scrolling();

        //}

        //void Scrolling()
        //{
        //    //horizontal

        //    int boundary = 380;
        //    int rightBoundary = 380;

        //    if (_player.x + x < boundary)
        //    {
        //        x = boundary - _player.x;
        //    }
        //    if(_player.x + x > game.width - rightBoundary)
        //    {
        //        x = game.width - rightBoundary - _player.x;
        //    }

        //    //vertical
        //    boundary -= 100;
        //    rightBoundary -= 100;
        //    if (_player.y + y < boundary)
        //    {
        //        y = boundary - _player.y;
        //    }
        //    if(_player.y + y > game.height - rightBoundary)
        //    {
        //        y = game.height - rightBoundary - _player.y;
        //    }
        //}
        #endregion
        #region custom loader
        //void SpawnTiles(Map levelData)
        //{
        //    if (levelData.Layers == null || levelData.Layers.Length == 0) return;

        //    Layer mainLayer = levelData.Layers[0];

        //    short[,] tileNumbers = mainLayer.GetTileArray();

        //    for (int row = 0; row < mainLayer.Height; row++)
        //        for (int col = 0; col < mainLayer.Width; col++)
        //        { 
        //            int tileNum = tileNumbers[col, row];

        //            TileSet tiles = levelData.GetTileSet(tileNum);

        //            if (tileNum > 0)
        //            {
        //                var tile = new CollisionTile(tiles.Image.FileName, tiles.Columns,tiles.Rows);//why not use normal sprite?
        //                tile.SetFrame(tileNum - tiles.FirstGId);
        //                tile.x = col * tile.width;
        //                tile.y = row * tile.height;
        //                AddChild(tile);
        //            }
        //        }

        //}

        //void SpawnObjects(Map levelData)
        //{ 
        //    if (levelData.ObjectGroups == null || levelData.ObjectGroups.Length == 0) return;

        //    var objectGroup = levelData.ObjectGroups[0];

        //    if (objectGroup.Objects == null || objectGroup.Objects.Length == 0) return;

        //    foreach (var obj in objectGroup.Objects)
        //    {
        //        Sprite newObj = null;
        //        switch (obj.Type)
        //        {
        //            case "Player":
        //                player = new Player();
        //                newObj = player;
        //            break;

        //        }
        //        if (newObj != null)
        //        {
        //            newObj.x = obj.X + newObj.width / 2;
        //            newObj.y = obj.Y - newObj.height / 2;
        //            AddChild(newObj);
        //        }
        //    }
        //}
        #endregion
    }
}
