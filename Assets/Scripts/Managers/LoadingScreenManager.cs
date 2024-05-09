using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Necroisle
{
    public class LoadingScreenManager : MonoBehaviour
    {
        public Slider progressBar;
        public TextMeshProUGUI progressText;

        private int totalFunctions;

        // Метод для инициализации загрузочного экрана
        public void InitializeLoadingScreen(int totalFunctions)
        {
            this.totalFunctions = totalFunctions;
        }

        // Метод для обновления прогресса загрузки

        public void UpdateLoadingScreen(float progress)
        {
            progressBar.value = progress;
        }

        public void UpdateLoadingScreen(string text)
        {
            progressText.text = text;
        }
    }
}
