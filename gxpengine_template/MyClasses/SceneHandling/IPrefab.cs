using GXPEngine;

public interface IPrefab //for objects that I want to dynamically spawn, not for things that I spawn in Tiled
{//don't forget to clone arrays and other refference types (Also children)
    GameObject Clone();
}
