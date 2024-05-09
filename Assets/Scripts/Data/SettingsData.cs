using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Necroisle
{
    public class SettingsData : MonoBehaviour
{
    public Toggle fps_counter;
    public Toggle audio_debugger;
    public Toggle system_info;
    public CanvasGroup fps_canvas;
    public CanvasGroup fps_alloc_canvas;
    public CanvasGroup audio_canvas;
    public CanvasGroup system_canvas;
    // Start is called before the first frame update
    void Start()
    {
        if (Tayx.Graphy.GraphyManager.Instance)
        {
            fps_canvas = FindObjectOfType<Tayx.Graphy.Fps.G_FpsManager>().GetComponent<CanvasGroup>();
            fps_alloc_canvas = FindObjectOfType<Tayx.Graphy.Ram.G_RamManager>().GetComponent<CanvasGroup>();
            audio_canvas = FindObjectOfType<Tayx.Graphy.Audio.G_AudioManager>().GetComponent<CanvasGroup>();
            system_canvas = FindObjectOfType<Tayx.Graphy.Advanced.G_AdvancedData>().GetComponent<CanvasGroup>();
        }
        if (GameSingleton.Instance.DebugFPS)
        {
            fps_counter.isOn = true;
        }
        if (GameSingleton.Instance.DebugAudio)
        {
            audio_debugger.isOn = true;
        }
        if (GameSingleton.Instance.DebugSystem)
        {
            system_info.isOn = true;
        }

        fps_counter.onValueChanged.AddListener((value) =>
                {
                    if (value)
                    {
                        fps_canvas.alpha = 1;
                        fps_alloc_canvas.alpha = 1;
                        GameSingleton.Instance.DebugFPS = true;
                    }
                    else
                    {
                        fps_canvas.alpha = 0;
                        fps_alloc_canvas.alpha = 0;
                        GameSingleton.Instance.DebugFPS = false;
                    }
                });

        audio_debugger.onValueChanged.AddListener((value) =>
                {
                    if (value)
                    {
                        audio_canvas.alpha = 1;
                        GameSingleton.Instance.DebugAudio = true;
                    }
                    else
                    {
                        audio_canvas.alpha = 0;
                        GameSingleton.Instance.DebugAudio = false;
                    }
                });

        system_info.onValueChanged.AddListener((value) =>
                {
                    if (value)
                    {
                        system_canvas.alpha = 1;
                        GameSingleton.Instance.DebugSystem = true;
                    }
                    else
                    {
                        system_canvas.alpha = 0;
                        GameSingleton.Instance.DebugSystem = false;
                    }
                });
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
}
