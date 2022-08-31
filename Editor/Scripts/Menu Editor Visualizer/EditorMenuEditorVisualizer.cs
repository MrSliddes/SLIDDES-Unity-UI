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

            selected.logDebugs = GUILayout.Toggle(selected.logDebugs, new GUIContent("Log Debugs", "Log the debug.logs of this script to the console"));
            selected.useCustomRectTransforms = GUILayout.Toggle(selected.useCustomRectTransforms, new GUIContent("Use Custom Rect Transforms", "Do you want to set your own RectTransforms instead of the script auto assigning them?"));
            if(selected.useCustomRectTransforms)
            {
                SerializedObject so = new SerializedObject(selected);
                SerializedProperty sp = so.FindProperty("menus");
                EditorGUILayout.PropertyField(sp, true);
                so.ApplyModifiedProperties();
            }
        }
    }
}
