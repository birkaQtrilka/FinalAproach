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
        public AnimationSprite CountText;

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
    readonly struct TextData
    {
        public readonly Vec2 Pos;
        public readonly int Frame;
        public readonly AnimationSprite Text;
        public TextData(Vec2 pos, int frame, AnimationSprite text)
        {
            Text = text;
            Pos = pos;
            Frame = frame;
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
            string numbersFileName = data.GetStringProperty("NumbersTileSetName");
            List<TextData> texts = new List<TextData>();

            foreach (string pair in data.GetStringProperty("PrefabNameAndCountCSV").Split(','))
            {
                if(string.IsNullOrEmpty(pair)) continue;
                Placeables.Add(new PlaceableData(pair.Split(':')));
                var newMenuImg = new Sprite(Placeables[i].PlaceablePrefab.MenuImg);
                var newTextMesh = new AnimationSprite(numbersFileName, 6, 2, -1, true, false);
                texts.Add(new TextData(head + head2, Placeables[i].Count - 1, newTextMesh));

                Placeables[i].Image = newMenuImg;
                Placeables[i].CountText = newTextMesh;

                AddChild(newMenuImg);
                newMenuImg.SetPosInVec2(head + head2);
                newMenuImg.SetOrigin(newMenuImg.width / 2, newMenuImg.height / 2);
                newMenuImg.rotation = Placeables[i].PlaceablePrefab.MenuImageRotation;

                var placeableClone = Placeables[i].PlaceablePrefab.Clone() ;
                MyUtils.MyGame.CurrentLevel.AddChild(placeableClone);
                placeableClone.visible = false;
                Vector2 clonePos = TransformPoint(newMenuImg.x, newMenuImg.y);
                placeableClone.SetXY(clonePos.x, clonePos.y);
                var placeable = (placeableClone as IPlaceable);
                placeable.Placed += OnItemPlaced;
                placeable.InInventory = true;
                placeable.MenuSprite = newMenuImg;

                head += Vec2.up * newMenuImg.height;
                i++;
            }
            foreach (var t in texts)
            {
                AddChild(t.Text);
                t.Text.SetPosInVec2(t.Pos);
                t.Text.SetFrame(t.Frame);
            }
        }

        private void OnItemPlaced(IPlaceable item)
        {
            item.Placed -= OnItemPlaced;
            GameObject goItem = item as GameObject;

            PlaceableData itemData = Placeables.First(x => (x.PlaceablePrefab as GameObject).name == goItem.name);
            item.InInventory = false;
            itemData.CountText.SetFrame(--itemData.Count - 1);

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
            clone.InInventory = true;
            clone.MenuSprite = itemData.Image;
            Vector2 clonePos = TransformPoint(itemData.Image.x, itemData.Image.y);
            goClone.SetXY(clonePos.x, clonePos.y);
            clone.Placed += OnItemPlaced;
        }

    }
}
