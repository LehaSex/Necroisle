using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// FPS Counter
    /// </summary>
    public class FPS : MonoBehaviour
    {
        private float average_delta = 0f;
        private GUIStyle style;

        private void Start()
        {
            style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = Screen.height / 50;
            style.normal.textColor = new Color(1f, 1f, 1f, 1f);
        }

        void Update()
        {
            float diff = Time.unscaledDeltaTime - average_delta;
            average_delta += diff * 0.2f;
        }

        void OnGUI()
        {
            float frame_rate = 1f / average_delta;
            string text = frame_rate.ToString("0") + " fps";

            Rect rect = new Rect(0, 0, Screen.width, Screen.height / 50);
            GUI.Label(rect, text, style);
        }
    }
}