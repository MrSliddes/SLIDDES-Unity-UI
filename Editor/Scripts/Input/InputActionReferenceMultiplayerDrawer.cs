using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Editor
{
    [CustomPropertyDrawer(typeof(InputActionReferenceMultiplayer))]
    public class InputActionReferenceMultiplayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(position, property.FindPropertyRelative("inputActionReference"), new GUIContent("Input Action Reference (Multiplayer)", "Get a callback from the input action reference, from any player that invokes the action"), true);

            EditorGUI.EndProperty();
        }
    }
}
