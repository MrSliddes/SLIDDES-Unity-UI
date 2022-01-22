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
            if(selected.useCustomRectTransforms)
            {
                if(GUILayout.Button("Organize Menus", GUILayout.Height(32)))
                {
                    selected.OrganiseMenus();
                }
                GUILayout.Space(8);
                if(GUILayout.Button("Reset Menus", GUILayout.Height(32)))
                {
                    selected.ResetMenus();
                }
                base.OnInspectorGUI();
            }
            else
            {
                if(GUILayout.Button("Organize Menus", GUILayout.Height(32)))
                {
                    selected.OrganiseMenus();
                }
                GUILayout.Space(8);
                if(GUILayout.Button("Reset Menus", GUILayout.Height(32)))
                {
                    selected.ResetMenus();
                }
                GUILayout.Space(8);
                selected.useCustomRectTransforms = GUILayout.Toggle(selected.useCustomRectTransforms, new GUIContent("Use Custom Rect Transforms", "Do you want to set your own RectTransforms instead of the script auto assigning them?"));
            }
        }
    }
}
