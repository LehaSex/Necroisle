using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    public class ResolutionInfo
    {
        public int width;
        public int height;
    }

    public class GameSingleton : Singleton<GameSingleton>
    {
        [SerializeField]
        private bool isPC = !TheGame.IsMobile();

        [SerializeField]
        private bool debugFPS = false;
        [SerializeField]
        private bool debugAudio = false;
        [SerializeField]
        private bool debugSystem = false;
        public ResolutionInfo resolutionInfo;
        public bool IsPC
        {
            get { return isPC; }
            set { isPC = value; }
        }
        public bool DebugAudio
        {
            get { return debugAudio; }
            set { debugAudio = value; }
        }
        public bool DebugSystem
        {
            get { return debugSystem; }
            set { debugSystem = value; }
        }
        public bool DebugFPS
        {
            get { return debugFPS; }
            set { debugFPS = value; }
        }

        public List<LUAObject> activeLuaObject;

        public void CallDebugLog(string message)
        {
            Debug.Log(message);
        }



        protected void Start()
        {
            resolutionInfo = new ResolutionInfo();
            resolutionInfo.width = Screen.width;
            resolutionInfo.height = Screen.height;
#if UNITY_EDITOR
            if (UnityEngine.Device.SystemInfo.deviceType != DeviceType.Desktop)
            {
                isPC = false;
            }
            else
            {
                isPC = true;
            }
#endif
        }

    }   
}
