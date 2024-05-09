using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Necroisle
{
    public class InitStartGame : MonoBehaviour
    {
        private LoadingScreenManager loadingScreenManager;
        private string loadingText;

        private void Start()
        {
            // loading screen manager in this object
            loadingScreenManager = GetComponent<LoadingScreenManager>();
            // Вызываем корутину для последовательного выполнения функций и обновления загрузочного экрана
            StartCoroutine(ExecuteCountableFunctions());
        }

        private IEnumerator ExecuteCountableFunctions()
        {
            // Получаем все методы класса
            var methods = typeof(InitStartGame).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            int totalFunctions = 0;
            foreach (var method in methods)
            {
                // Проверяем наличие атрибута CountableFunction
                var attribs = method.GetCustomAttributes(typeof(CountableFunctionAttribute), true);
                if (attribs.Length > 0)
                {
                    totalFunctions++;
                }
            }
            Debug.Log("[AppData] Total functions to complete: " + totalFunctions);

            // Инициализируем загрузочный экран
            loadingScreenManager.InitializeLoadingScreen(totalFunctions);

            // Постепенно выполняем все методы с атрибутом CountableFunction
            int completedFunctions = 0;
            foreach (var method in methods)
            {
                var attribs = method.GetCustomAttributes(typeof(CountableFunctionAttribute), true);
                if (attribs.Length > 0)
                {
                    // Вызываем метод
                    method.Invoke(this, null);
                    completedFunctions++;
                    // Обновляем загрузочный экран
                    loadingScreenManager.UpdateLoadingScreen((float)completedFunctions / totalFunctions);
                    loadingScreenManager.UpdateLoadingScreen(loadingText);
                    // Ждем небольшую задержку между выполнением методов
                    yield return new WaitForSeconds(0.1f);
                }
            }

            Debug.Log("[AppData] All functions completed");

            // Асинхронно переходим на сцену MainMenu
            yield return LoadMainMenuAsync();
        }

        private IEnumerator LoadMainMenuAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

            // Ждем пока сцена полностью загрузится
            while (!asyncLoad.isDone)
            {
                // Обновляем прогресс загрузки на загрузочном экране
                loadingScreenManager.UpdateLoadingScreen(asyncLoad.progress);
                yield return null;
            }
        }

        [CountableFunction]
        private void DeterminePlatform()
        {
            loadingText = "Определение платформы";
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.Log("[AppData] Current Platform: Windows");
                GameSingleton.Instance.IsPC = true;
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.Log("[AppData] Current Platform: Mobile");
                GameSingleton.Instance.IsPC = false;
            }
            else
            {
                Debug.LogWarning("[AppData] Current Platform: Unknown platform");
                GameSingleton.Instance.IsPC = false;
            }
        }

        [CountableFunction]
        private void GenerateFolders()
        {
            loadingText = "Проверка данных";
            // create folders if not exist
            if (!Directory.Exists(Application.persistentDataPath + "/lua"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/lua");
            }
        }

        [CountableFunction]
        private void InitPlayerControls()
        {
            loadingText = "Запуск игрового контроллера";
            
        }
    }
}
