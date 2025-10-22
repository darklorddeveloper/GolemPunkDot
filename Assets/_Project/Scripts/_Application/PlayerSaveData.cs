using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkLordGame
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public int selectedLanguage = 0;

        public float bgmVolume = 0.4f;

        public float sfxVolume = 0.6f;

        public bool isFullScreen = true;

        public CurrentRunData currentRunData;
        public int selectedClassID;
    }

    [System.Serializable]
    public class CurrentRunData
    {
        public bool isRunning = false;
        public int selectedClassID;
    }
}
