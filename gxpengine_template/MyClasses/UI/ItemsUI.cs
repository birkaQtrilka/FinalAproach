using GXPEngine;
using GXPEngine.Core;
using gxpengine_template.MyClasses.Environment;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.UI
{
    class PlaceableData
    {
        public int Count;
        public readonly IPlaceable PlaceablePrefab;
        public Sprite Image;
        public TextMesh CountText;

        public PlaceableData(int count, IPlaceable placeable)
        {
            Count = count;
            PlaceablePrefab = placeable;
        }
        public PlaceableData(string[] pair)
        {
            Image = null;
            CountText = null;
            PlaceablePrefab = MyUtils.MyGame.Prefabs[pair[0]] as IPlaceable;
            Count = int.Parse(pair[1]);
        }
    }

    public class ItemsUI : AnimationSprite
    {
        List<PlaceableData> Placeables = new List<PlaceableData>();

        public ItemsUI(string filename, int cols, int rows, TiledObject data) : base(filename, cols, rows, -1,true,false)
        {
            AddChild(new Coroutine(Init(data)));

            alpha = 0;
        }

        IEnumerator Init(TiledObject data)
        {
            yield return null;
            int i = 0;
            //SetOrigin(width / 2, height / 2);
            Vec2 head2 = new Vec2(-width/2,-height/2);
            Vec2 head = new Vec2(0, 0);

            foreach (string pair in data.GetStringProperty("PrefabNameAndCountCSV").Split(','))
            {
                if(string.IsNullOrEmpty(pair)) continue;
                Placeables.Add(new PlaceableData(pair.Split(':')));
                var newMenuImg = new Sprite(Placeables[i].PlaceablePrefab.MenuImg);
                var newTextMesh = new TextMesh(Placeables[i].Count.ToString(), newMenuImg.width, newMenuImg.width);
                var placeableClone = Placeables[i].PlaceablePrefab.Clone() ;

                newTextMesh.TextSize = data.GetIntProperty("TextSize");
                newTextMesh.TextColor = Color.White;
                Placeables[i].Image = newMenuImg;
                Placeables[i].CountText = newTextMesh;

                AddChild(newMenuImg);
                AddChild(newTextMesh);
                newTextMesh.SetPosInVec2(head + head2);
                newMenuImg.SetPosInVec2(head + head2);
                newMenuImg.SetOrigin(newMenuImg.width / 2, newMenuImg.height / 2);
                newMenuImg.rotation = Placeables[i].PlaceablePrefab.MenuImageRotation;

                MyUtils.MyGame.CurrentLevel.AddChild(placeableClone);
                placeableClone.visible = false;
                Vector2 clonePos = TransformPoint(newMenuImg.x, newMenuImg.y);
                placeableClone.SetXY(clonePos.x, clonePos.y);
                (placeableClone as IPlaceable).Placed += OnItemPlaced;

                newTextMesh.HorizontalAlign = CenterMode.Max;
                newTextMesh.VerticalAlign = CenterMode.Max;
                //newTextMesh.SetXY(newMenuImg.width / 2, newMenuImg.height / 2);
                head += Vec2.up * newMenuImg.height;
                i++;
            }
        }

        private void OnItemPlaced(IPlaceable item)
        {
            item.Placed -= OnItemPlaced;
            GameObject goItem = item as GameObject;

            PlaceableData itemData = Placeables.First(x => (x.PlaceablePrefab as GameObject).name == goItem.name);
            
            itemData.CountText.Text = (--itemData.Count).ToString();

            if(itemData.Count == 0)
            {
                itemData.Image.Destroy();
                itemData.CountText.Destroy();
                Placeables.Remove(itemData);
                return;
            }

            GameObject goClone = item.Clone();
            goClone.visible = false;
            IPlaceable clone = goClone as IPlaceable;
            MyUtils.MyGame.CurrentLevel.AddChild(goClone);
            Vector2 clonePos = TransformPoint(itemData.Image.x + itemData.Image.width * .5f, itemData.Image.y + itemData.Image.height * .5f);
            goClone.SetXY(clonePos.x, clonePos.y);
            clone.Placed += OnItemPlaced;
        }

        //IEnumerator Init(TiledObject data)
        //{
        //    yield return null;

        //    var paddingL = data.GetFloatProperty("PaddingL", 0.1f) * width;
        //    var paddingR = data.GetFloatProperty("PaddingR", 0.1f) * width;
        //    var paddingT = data.GetFloatProperty("PaddingT", 0.1f) * height;
        //    var paddingB = data.GetFloatProperty("PaddingB", 0.1f) * height;
        //    var spacingX = data.GetFloatProperty("SpacingX", 0.1f) * width;
        //    var spacingY = data.GetFloatProperty("SpacingY", 0.1f) * height;

        //    Vec2 pos = new Vec2(x, y);

        //    SetOrigin(0, 0);
        //    SetXY(pos.x, pos.y);
        //    alpha = data.GetFloatProperty("BgAlpha", 1);

        //    int mazeColumns = data.GetIntProperty("Columns",1);
        //    int mazeRows = data.GetIntProperty("Rows", 4);
        //    int pieceW = Mathf.Floor((width - paddingL - paddingR - (spacingX * (mazeColumns - 1))) / mazeColumns);
        //    int pieceH = Mathf.Floor((height - paddingT - paddingB - (spacingY * (mazeRows - 1))) / mazeRows);
        //    Vector2 offset = new Vector2(pieceW * .5f + paddingL, pieceH * .5f + paddingT);
        //    Vector2 currSpacing = new Vector2();

        //    for (int i = 0; i < _pieces.Length; i++)
        //    {
        //        var piece = PieceFactory(_mazeLogic.Pieces[i].Type, pieceSpriteSheetPath, ssColumns, ssRows);
        //        piece.SetOrigin(piece.width / 2, piece.height / 2);
        //        _container.AddChild(piece);
        //        _pieces[i] = piece;

        //        piece.width = pieceW;
        //        piece.height = pieceH;

        //        currSpacing.x = spacingX * (i % mazeColumns);
        //        currSpacing.y = spacingY * (i / mazeColumns);
        //        piece.SetXY(pieceW * (i % mazeColumns) + offset.x + currSpacing.x, pieceH * (i / mazeColumns) + offset.y + currSpacing.y);
        //    }

        //}
    }
}
