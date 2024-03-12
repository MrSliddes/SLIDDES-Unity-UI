using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SLIDDES.UI.Editor
{
    //[CustomPropertyDrawer(typeof(Transitions))]
    public class TransitionsDrawer : PropertyDrawer
    {
        VisualElementGUI visualElementGUI;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            VisualElementGUI visual = new VisualElementGUI(position, property, label);

            visual.Add(property.FindPropertyRelative("types"));
            //visual.Add(property.FindPropertyRelative("animation"));

            visualElementGUI = visual;

            visual.Draw();

            //rect = visual.Draw();
        }

    }
}
