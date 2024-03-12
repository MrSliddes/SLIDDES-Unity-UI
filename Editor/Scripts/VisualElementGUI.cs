using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SLIDDES.UI.Editor
{
    public class VisualElementGUI
    {
        private int childCount;
        private GUIContent label;
        private Rect position;
        private SerializedProperty property;
        private List<SerializedProperty> properties = new List<SerializedProperty>();

        private bool editorFoldout;

        public VisualElementGUI() { }

        public VisualElementGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.position = position;
            this.label = label;
            this.property = property;
        }

        public float GetHeight(SerializedProperty property)
        {
            return childCount * EditorGUIUtility.singleLineHeight;
        }

        public void Add(SerializedProperty property)
        {
            properties.Add(property);
            childCount++;
        }

        public Rect Draw()
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if(property.isExpanded)
            {
                EditorGUI.indentLevel++;

                for(int i = 0; i < properties.Count; i++)
                {
                    SerializedProperty property = properties[i];

                    position.y += EditorGUIUtility.singleLineHeight;
                    //position.height += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position, property, true);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
            return position;
        }

    }
}
