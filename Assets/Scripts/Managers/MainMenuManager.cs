using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace Necroisle
{
    /// <summary>
    /// Manages the main menu UI
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject MenuPanel;
        public GameObject CharactersPanel;
        public GameObject SettingsPanel;
        public GameObject PlayPanel;
        public GameObject LuaPanel;
        public GameObject LoadingPanel;
        public Button PlayButton;
        public Button LuaRefreshButton;
        public Button CharactersButton;
        public Button SettingsButton;
        public Button VKButton;
        public Button ExitButton;
        public Button BackFromSettingsButton;
        public Button BackFromLuaButton;
        public Button BackFromCharacters;
        public Button LuaButton;
        public TextMeshProUGUI Header;
        public GameObject CurrentPanel;
        public LoadingScreenManager loadingScreenManager;
        private Stack<GameObject> panelStack = new Stack<GameObject>();

        public enum ScreenType
        {
            Play,
            Characters,
            Settings,
            Menu,
            Lua,
            Loading
        }

        private Dictionary<ScreenType, GameObject> screenPanels = new Dictionary<ScreenType, GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            PlayButton.onClick.AddListener(GoToPlay);
            CharactersButton.onClick.AddListener(GoToCharacters);
            SettingsButton.onClick.AddListener(GoToSettings);
            LuaButton.onClick.AddListener(GoToLua);

            BackFromSettingsButton.onClick.AddListener(GoBack);
            BackFromLuaButton.onClick.AddListener(GoBack);
            BackFromCharacters.onClick.AddListener(GoBack);

            VKButton.onClick.AddListener(GoToVK);
            ExitButton.onClick.AddListener(GoToExit);
            CurrentPanel = MenuPanel;

            // Initialize the dictionary with panel references
            screenPanels.Add(ScreenType.Play, PlayPanel);
            screenPanels.Add(ScreenType.Characters, CharactersPanel);
            screenPanels.Add(ScreenType.Settings, SettingsPanel);
            screenPanels.Add(ScreenType.Menu, MenuPanel);
            screenPanels.Add(ScreenType.Lua, LuaPanel);
            screenPanels.Add(ScreenType.Loading, LoadingPanel);

            Debug.Log("[AppData] " + Application.persistentDataPath);
        }

        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(GoToPlay);
            CharactersButton.onClick.RemoveListener(GoToCharacters);
            SettingsButton.onClick.RemoveListener(GoToSettings);
            VKButton.onClick.RemoveListener(GoToVK);
            ExitButton.onClick.RemoveListener(GoToExit);
            BackFromSettingsButton.onClick.RemoveListener(GoBack);
            BackFromLuaButton.onClick.RemoveListener(GoBack);
            BackFromCharacters.onClick.RemoveListener(GoBack);
            LuaButton.onClick.RemoveListener(GoToLua);
            
        }

        void GoToPlay()
        {
            // Hide header
            Header.gameObject.SetActive(false);
            ChangePanel(ScreenType.Loading);
            loadingScreenManager.InitializeLoadingScreen(1);
            StartCoroutine(GoToPlayRoutine());
        }

        private IEnumerator GoToPlayRoutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WorldGenMap");

            // Ждем пока сцена полностью загрузится
            while (!asyncLoad.isDone)
            {
                loadingScreenManager.UpdateLoadingScreen(asyncLoad.progress);
                yield return null;
            }
        }

        void GoToCharacters()
        {
            ChangePanel(ScreenType.Characters);
        }

        void GoToSettings()
        {
            ChangePanel(ScreenType.Settings);
        }

        void GoToLua()
        {
            ChangePanel(ScreenType.Lua);
        }

        /// <summary>
        /// Goes back to the previous panel
        /// </summary>
        void GoBack()
        {
            if (panelStack.Count > 0)
            {
                GameObject previousPanel = panelStack.Pop();
                CurrentPanel.SetActive(false);
                previousPanel.SetActive(true);
                CurrentPanel = previousPanel;
            }
        }

        void GoToVK()
        {
            Application.OpenURL("https://vk.com/idi.nahooy");
        }

        /// <summary>
        /// Changes the active panel to the one specified by the screen type
        /// </summary>

        void ChangePanel(ScreenType screenType)
        {
            if (screenPanels.ContainsKey(screenType))
            {
                panelStack.Push(CurrentPanel);
                CurrentPanel.SetActive(false);
                screenPanels[screenType].SetActive(true);
                CurrentPanel = screenPanels[screenType];
            }
            else
            {
                Debug.LogError("Panel for screen type " + screenType + " is not defined.");
            }
        }



        void GoToExit()
        {
            Application.Quit();
            Debug.Log("Exit");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
