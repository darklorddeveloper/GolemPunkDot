using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace DarkLordGame
{
    [CustomPropertyDrawer(typeof(GUIDAttribute))]
    public class GUIDAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            string assetPath = AssetDatabase.GUIDToAssetPath(property.stringValue);
            position.height = EditorGUIUtility.singleLineHeight;

            if (property.hasMultipleDifferentValues)
            {
                var targetMultiple = (GameObject)EditorGUI.ObjectField(position, property.displayName, null, typeof(GameObject), false);
                if (targetMultiple != null)
                {
                    property.stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(targetMultiple));

                }
            }
            else
            {
                GameObject gameObj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                gameObj = (GameObject)EditorGUI.ObjectField(position, property.displayName, gameObj, typeof(GameObject), false);
                if (gameObj != null)
                {
                    property.stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(gameObj));

                }
                else
                {
                    property.stringValue = AssetDatabase.AssetPathToGUID(assetPath);

                }
            }
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.LabelField(position, property.stringValue);
            EditorGUI.EndDisabledGroup();
        }

        

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2.5f;
        }
    }
}
#endif