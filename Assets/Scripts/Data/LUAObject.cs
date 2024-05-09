using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    public class LUAObject : MonoBehaviour
    {
        private string luaFileName;  // Имя Lua-файла
        private string luaFilePath;  // Путь к Lua-файлу
        private string luaText;     // Текст Lua-файла
        // lua text asset
        private TextAsset luaTextAsset;

        public void SetLuaFileName(string name)
        {
            luaFileName = name;
        }

        public void SetLuaFilePath(string path)
        {
            luaFilePath = path;
        }

        public void SetLuaTextAsset(TextAsset textAsset)
        {
            luaTextAsset = textAsset;
        }

        public string GetLuaFileName()
        {
            return luaFileName;
        }

        public string GetLuaFilePath()
        {
            return luaFilePath;
        }

        public TextAsset GetLuaTextAsset()
        {
            return luaTextAsset;
        }

        private void FindLuaName()
        {
            luaFileName = System.IO.Path.GetFileNameWithoutExtension(luaFilePath);
        }

        public void CreateTextAsset()
        {
            luaText = System.IO.File.ReadAllText(luaFilePath);
            // set text asset
            luaTextAsset = new TextAsset(luaText);
            FindLuaName();
        }
    }
}
