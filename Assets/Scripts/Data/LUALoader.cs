using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace Necroisle
{
    public class LUALoader : MonoBehaviour
    {
        public GameObject luaObjectPrefab;   // Префаб LUAObject
        private string[] luaFiles;            // Массив путей к LUA-файлам

        public Button LuaRefreshButton;       // Кнопка обновления списка LUA-файлов
        public Button OpenLuaFolderButton;    // Кнопка открытия папки с LUA-файлами
        // List of buttons to execute lua files
        public List<Toggle> LuaExecuteButtons;


        void Start()
        {
            // check if GameSingleton exists and is PC
            if (GameSingleton.Instance.IsPC)
            {
                OpenLuaFolderButton.gameObject.SetActive(true);
                OpenLuaFolderButton.onClick.AddListener(OpenLuaFolder);
            }
            else
            {
                OpenLuaFolderButton.gameObject.SetActive(false);
            }
            LuaRefreshButton.onClick.AddListener(LoadLuaFiles);
            LoadLuaFiles();
        }

        void LoadLuaFiles()
        {
            // Remove all children prefabs 
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            string luaFolderPath = Application.persistentDataPath + "/lua"; // Путь к папке Lua в persistent data path

            if (!Directory.Exists(luaFolderPath))
            {
                Debug.LogError("Lua folder not found at: " + luaFolderPath);
                return;
            }

            luaFiles = Directory.GetFiles(luaFolderPath, "*.lua"); // Получаем все Lua-файлы в папке Lua

            foreach (string luaFilePath in luaFiles)
            {
                string luaFileName = Path.GetFileNameWithoutExtension(luaFilePath); // Получаем имя Lua-файла без расширения

                //instantiate as a child of the this game object
                GameObject luaObject = Instantiate(luaObjectPrefab, transform);
                // add button to array
                LuaExecuteButtons.Add(luaObject.GetComponentInChildren<Toggle>());
                // add listener to the button if checked Execute if unchecked StopExecution
                //LuaExecuteButtons[LuaExecuteButtons.Count - 1].onValueChanged.AddListener(delegate { ExecuteLuaFile(luaFilePath); });
                LuaExecuteButtons[LuaExecuteButtons.Count - 1].onValueChanged.AddListener((value) =>
                {
                    if (value)
                    {
                        ExecuteLuaFile(luaFilePath);
                    }
                    else
                    {
                        StopExecution(luaFilePath);
                    }
                });
                
                
                // Находим компонент TextMeshPro в дочернем объекте
                TextMeshProUGUI luaNameText = luaObject.GetComponentInChildren<TextMeshProUGUI>();
                // Устанавливаем имя Lua-файла в компонент TextMeshPro
                // if name > 18 characters, cut it and add "..."
                if (luaFileName.Length > 18)
                {
                    luaNameText.text = luaFileName.Substring(0, 15) + "...";
                }
                else
                {
                    luaNameText.text = luaFileName;
                }

                // проверяем наличие LUAObject с таким именем уже на сцене

                LUAObject[] luaObjects = FindObjectsOfType<LUAObject>();
                foreach (LUAObject luaObjectComponent in luaObjects)
                {
                    if (luaObjectComponent.GetLuaFilePath() == luaFilePath)
                    {
                        LuaExecuteButtons[LuaExecuteButtons.Count - 1].isOn = true;
                    }
                }
            }
        }

        void OpenLuaFolder()
        {
            string luaFolderPath = Application.persistentDataPath + "/lua"; // Путь к папке Lua в persistent data path
            System.Diagnostics.Process.Start(luaFolderPath);
        }

        // get lua files 
        public string[] GetLuaFiles()
        {
            return luaFiles;
        }

        // execute lua file
        public void ExecuteLuaFile(string luaFilePath)
        {
            // create new game object
            GameObject luaObject = new GameObject();
            // add LUAObject component
            LUAObject luaObjectComponent = luaObject.AddComponent<LUAObject>();
            // set lua file path
            luaObjectComponent.SetLuaFilePath(luaFilePath);
            // create text asset
            luaObjectComponent.CreateTextAsset();
            luaObject.AddComponent<ModInjector>();
        }

        public void StopExecution(string luaFilePath)
        {
            LUAObject[] modInjectors = FindObjectsOfType<LUAObject>();
            // find the one with the same lua file path
            foreach (LUAObject modInjector in modInjectors)
            {
                if (modInjector.GetLuaFilePath() == luaFilePath)
                {
                    Destroy(modInjector.gameObject);
                }
            }            
        }

        
    }
}
