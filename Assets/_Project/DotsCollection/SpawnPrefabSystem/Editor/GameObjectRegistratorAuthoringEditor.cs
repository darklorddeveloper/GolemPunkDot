using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace DarkLordGame
{
    [CustomEditor(typeof(PrefabPoolAuthoring))]
    public class GameObjectRegistratorAuthoringEditor : Editor
    {
        private string text = "";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            GUILayout.Label("Text to compare");
            text = GUILayout.TextField(text);

            if (GUILayout.Button("Check inside the string"))
            {
                var textToCompare = text.ToLower();
                var authoring = target as PrefabPoolAuthoring;
                bool found = false;
                for (int i = 0; i < authoring.prefabs.Count; i++)
                {
                    if (authoring.prefabs[i].name.ToLower() == textToCompare)
                    {
                        Debug.Log("found");
                        found = true;
                    }
                }
                if(found == false)
                {
                    Debug.Log("NOT FOUND");
                    return;
                }
            }
        }
    }
}
