using UnityEngine;
using UnityEditor;

namespace DarkLordGame
{
    [CustomPropertyDrawer(typeof(IDGenerator))]
    internal sealed class IDGeneratorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IDGenerator idGenerator = (IDGenerator)attribute;
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                position.height = EditorGUIUtility.singleLineHeight;
                property.serializedObject.Update();
                EditorGUI.PropertyField(position, property);
                position.y += EditorGUIUtility.singleLineHeight + 10;
                position.height = EditorGUIUtility.singleLineHeight * 1.5f;
                if (GUI.Button(position, "Generate UniqueID"))
                {
                    property.intValue = IDGenerateHelper.GenerateID(idGenerator.saveName, idGenerator.maxNumbers);
                    property.serializedObject.ApplyModifiedProperties();
                }
                
            }
        }
        //戻り値として返した値が GUI の高さとして使用されるようになる
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            return height * 3;
        }
    }


}