using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Editor
{
    [CustomPropertyDrawer(typeof(InputSystemIAR))]
    public class InputSystemIARDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(position, property.FindPropertyRelative("type"), new GUIContent("Input System IAR", "Attach a callback to InputSystemUIInputModule based on selected type"), true);

            EditorGUI.EndProperty();
        }
    }
}
