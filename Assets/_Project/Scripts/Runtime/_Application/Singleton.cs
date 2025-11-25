
using System.IO;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace DarkLordGame
{
    public class Singleton
    {
        public static Singleton instance;
        public PlayerSaveData playerSaveData;
        public ResourcesData resourcesData;
        public const string PlayerSaveDataName = "Save.Json";
        public const string CurrentRunSaveDataName = "CurrentRunSaveV1.Json";
        public static readonly string SavePath = Application.persistentDataPath + "/" + PlayerSaveDataName;
        public static readonly string CurrentRunSavePath = Application.persistentDataPath + "/" + CurrentRunSaveDataName;

        public static void Init()
        {
            if (instance == null)
            {
                instance = new Singleton();
                instance.LoadSave();
            }
        }

        public void LoadSave()
        {
            if (System.IO.File.Exists(SavePath))
            {
                StreamReader reader = new StreamReader(SavePath);
                var data = reader.ReadToEnd();
                playerSaveData = JsonUtility.FromJson<PlayerSaveData>(data);
                Debug.Log($"loaded {SavePath}");
                reader.Dispose();
            }
            else
            {
                playerSaveData = new PlayerSaveData();
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Japanese:
                        playerSaveData.selectedLanguage = 1;
                        break;

                    case SystemLanguage.Chinese:
                    case SystemLanguage.ChineseSimplified:
                    case SystemLanguage.ChineseTraditional:
                        playerSaveData.selectedLanguage = 2;
                        break;
                    case SystemLanguage.French:
                        playerSaveData.selectedLanguage = 3;
                        break;
                    case SystemLanguage.German:
                        playerSaveData.selectedLanguage = 4;
                        break;
                    case SystemLanguage.English:
                    default:
                        playerSaveData.selectedLanguage = 0;
                        break;
                }
                Save();
            }


            resourcesData = Resources.Load<ResourcesData>("ResourcesData");
            resourcesData.Init();

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[playerSaveData.selectedLanguage];
        }

        public void SetFullScreen(bool fullScreen)
        {

            playerSaveData.isFullScreen = true;
            Save();
            UpdateFullScreen();
        }

        public void UpdateFullScreen()
        {
            var fullScreen = playerSaveData.isFullScreen;

            Screen.fullScreen = fullScreen;

            if (fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }

        public void Save()
        {
            string data = JsonUtility.ToJson(playerSaveData);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + PlayerSaveDataName, data);
        }

        public void ResetCurrentPlayData()
        {
            playerSaveData.currentRunData = new CurrentRunData();
            playerSaveData.currentRunData.selectedClassID = playerSaveData.selectedClassID;
            Save();
        }

        public void SaveCurrentPlay(GameStat gameStat)
        {
            //export
        }

        public GolemClass GetCurrentGolemClass()
        {
            return resourcesData.GetClassID(playerSaveData.selectedClassID);
        }
    }
}
