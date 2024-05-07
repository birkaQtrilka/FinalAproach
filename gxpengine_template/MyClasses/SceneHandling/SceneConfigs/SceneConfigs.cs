using GXPEngine;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template.MyClasses.SceneHandling
{
    public readonly struct LayerConfig
    {
        public readonly string ParentName;
        public readonly string LayerName;

        public LayerConfig(string parentName, string layerName)
        {
            ParentName = parentName;
            LayerName = layerName;
        }
        public LayerConfig(string[] arr)
        {
            ParentName = arr[0];
            LayerName = arr[1];
        }
    }

    public abstract class SceneConfigs : AnimationSprite
    {

        protected Level level;
        public LayerConfig[] ObjectLayers;

        public SceneConfigs(TiledObject data) : base("Assets/square.png", 1,1,-1,true,false)
        {
            visible = false;
            ObjectLayers = data.GetStringProperty("ObjectLayersCSV").Split(',').Select(x => new LayerConfig(x.Split(':'))).ToArray();
        }

        public void Init(Level level)
        {
            this.level = level;
            Initialize(level);
        }

        protected abstract void Initialize(Level level);

    }
}
