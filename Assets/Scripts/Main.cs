using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private LuaState lua = null;
    LuaFunction luaFunc = null;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);
        string luaPath = Application.dataPath + "/Lua";
        lua.AddSearchPath(luaPath);
        lua.DoFile("TestLua.lua");
        Callfunc("Control.Start", gameObject);
    }

    private void Callfunc(string func, GameObject obj)
    {
        luaFunc = lua.GetFunction(func);
        luaFunc.Call();
        luaFunc.Dispose();
        luaFunc = null;
    }

    private void OnApplicationQuit()
    {
        lua.Dispose();
        lua = null;
    }

    // Update is called once per frame
    void Update()
    {
        Callfunc("Control.Update", gameObject);
    }

    
}
