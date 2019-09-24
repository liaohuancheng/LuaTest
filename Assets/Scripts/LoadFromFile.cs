using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadFromFile : MonoBehaviour
{
    private int version;
    private LuaState lua = null;
    LuaFunction luaFunc = null;
    // Start is called before the first frame update
    void Start()
    {
        version = 3;
        LoadLua();
        LoadPrefabs();
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);
        string luaPath = Application.dataPath + "/Lua";
        lua.AddSearchPath(luaPath);
        lua.DoFile("TestLua.lua");
        Callfunc("Control.Start");
    }

    // Update is called once per frame
    void Update()
    {
        Callfunc("Control.Update");
    }

    void LoadPrefabs()
    {
        string path =   Application.streamingAssetsPath + "/player.unity3d";
        //AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(path));
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        //WWW www = WWW.LoadFromCacheOrDownload(path, 1);
        //AssetBundle ab = www.assetBundle;
        Object[] obj = ab.LoadAllAssets<GameObject>();

        LoadManifesWallpaper();
        foreach (var o in obj)
        {
            Instantiate(o);
        }
    }

    void LoadManifesWallpaper()
    {
        string path = Application.streamingAssetsPath + "/StreamingAssets";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] strs = manifest.GetAllDependencies("player.unity3d");

        foreach(var name in strs)
        {
            print(name);
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + name);
        }
    }

    void LoadLua()
    {
        string path = Application.streamingAssetsPath + "/lua.unity3d";
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        TextAsset text = ab.LoadAsset("TestLua.lua") as TextAsset;

        if (File.Exists(Application.dataPath + "/Lua/TestLua.lua") ) {
            File.Delete(Application.dataPath + "/Lua/TestLua.lua");
        }

        File.WriteAllBytes(Application.dataPath + "/Lua/TestLua.lua",text.bytes);
    }

    void StartLua()
    {
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);
        string luaPath = Application.dataPath + "/Lua";
        lua.AddSearchPath(luaPath);
        lua.DoFile("TestLua.lua");
        Callfunc("Control.Start");
    }

    private void Callfunc(string func)
    {
        luaFunc = lua.GetFunction(func);
        luaFunc.Call();
    }
}

