using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.MenuEditorVisualizer
{
    [CustomEditor(typeof(MenuEditorVisualizer))]
    public class EditorMenuEditorVisualizer : Editor
    {
        private MenuEditorVisualizer selected;

        private void OnEnable()
        {
            selected = (MenuEditorVisualizer)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Organize Menus", GUILayout.Height(32)))
            {
                selected.OrganiseMenus();
            }
            if(GUILayout.Button("Reset Menus", GUILayout.Height(32)))
            {
                selected.ResetMenus();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("menuPrefix"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debug"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useCustomRectTransforms"));

            if(selected.useCustomRectTransforms)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("menus"), true);             
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
