using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

namespace Necroisle
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class ModInjector : MonoBehaviour
    {
        public TextAsset luaScript;
        public Injection[] injections;

        private static ModInjector instance;
        public static ModInjector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ModInjector>();
                    if (instance == null)
                    {
                        GameObject apiObject = new GameObject("ModInjector");
                        instance = apiObject.AddComponent<ModInjector>();
                    }
                }
                return instance;
            }
        }

        private LuaEnv luaEnv;
        private float lastGCTime = 0;
        private const float GCInterval = 1; //1 second 

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;

        private LuaTable scriptEnv;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this && instance.name == this.name)
            {
                Destroy(gameObject);
                return;
            }

            luaEnv = new LuaEnv();
            scriptEnv = luaEnv.NewTable();

            // Main Injections for Lua
            injections = new Injection[1];
            injections[0] = new Injection();
            injections[0].name = "GameSingleton";
            injections[0].value = FindObjectOfType<GameSingleton>().gameObject;

            // find LUAObject on this game object
            LUAObject luaObject = GetComponent<LUAObject>();
            if (luaObject != null)
            {
                luaScript = luaObject.GetLuaTextAsset();
                this.name = luaObject.GetLuaFileName();
            }

            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }

            luaEnv.DoString(luaScript.text, "LuaTestScript", scriptEnv);

            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);

            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv.Dispose();
            injections = null;

            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
