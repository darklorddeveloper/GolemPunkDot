#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DarkLordGame
{
    public static class IDGenerateHelper
    {
        public static int GenerateID(string SaveName, int MaxID = 100000)
        {
            int number = Random.Range(0, MaxID);
            bool gotUniqueNumber = false;
            while (!gotUniqueNumber)
            {
                int findCached = EditorPrefs.GetInt(SaveName + number, -1);
                gotUniqueNumber = findCached == -1;
            }
            EditorPrefs.SetInt(SaveName + number, number);
            return number;
        }
    }
}
#endif