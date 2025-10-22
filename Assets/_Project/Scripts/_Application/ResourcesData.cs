using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
namespace DarkLordGame
{
    [CreateAssetMenu(fileName = "ResourcesData", menuName = "ResourcesData")]
    public class ResourcesData : ScriptableObject
    {
        public List<GolemClass> golemClasses = new();
        public void Init()
        {
        }

        public GolemClass GetClassID(int id)
        {
            for (int i = 0, length = golemClasses.Count; i < length; i++)
            {
                if (golemClasses[i].classID == id)
                {
                    return golemClasses[i];
                }
            }
            return null;
        }

    }
}
