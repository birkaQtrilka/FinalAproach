using System;
using System.Collections.Generic;
using GXPEngine;
using gxpengine_template;
using TiledMapParser;

public class MyGame : Game 
{
    public event Action BeforeLevelReload;
    public Level CurrentLevel { get; private set; }
    public Dictionary<string, IPrefab> Prefabs { get; private set; }

    string _newLevelName;

    public readonly DateTime StartTime;

    static void Main()
    {
        new MyGame().Start();
    }

    public MyGame() : base(1080,1920, false, true,500,1040,false)
	{
        name = "Main";
        Prefabs = LoadPrefabs();
        targetFps = 60;
        //to show how fast you've beat the game at the end
        StartTime = DateTime.Now;
        
        LoadLevel("Assets/Test(1.3).tmx");

        OnAfterStep += LoadSceneIfNotNull;
	}
    //for fast testing
    void Update()
    {
        if (Input.GetKeyDown(Key.R))// hot reload
        {
            LoadLevel(CurrentLevel.name);
        }
    }

    Dictionary<string, IPrefab> LoadPrefabs()
    {
        var prefabsDictionary = new Dictionary<string, IPrefab>();
        var loader = new TiledLoader("Assets/Prefabs.tmx", MyGame.main, false, autoInstance: true);
        loader.LoadObjectGroups();
        foreach (var obj in FindObjectsOfType<GameObject>())
            if (obj is IPrefab prefab)
            {
                RemoveChild(obj);
                prefabsDictionary.Add(obj.name, prefab);
            }
        return prefabsDictionary;
    }

    private void LoadSceneIfNotNull()
    {
        if (_newLevelName == null) return;
        DestroyAll();
        var level = new Level(_newLevelName);
        CurrentLevel = level;
        AddChild(level);
        level.Init();

        _newLevelName = null;
    }

    public void LoadLevel(string levelName)
    {
        _newLevelName = levelName;
    }

    public void ReloadLevel()
    {
        _newLevelName = CurrentLevel.name;
        BeforeLevelReload?.Invoke();
    }

    void DestroyAll()
    {
        foreach (var child in GetChildren())
        {
            if(!(child is INonDestroyable))
            {
                child.Destroy();
            }
        }
    }

    protected override void OnDestroy()
    {
        OnAfterStep -= LoadSceneIfNotNull;

    }
}