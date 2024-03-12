using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Menus.Editor
{
    [CustomEditor(typeof(Menu))]
    public class MenuEditor : UnityEditor.Editor
    {
        private SerializedProperty propertyCloseOnStart;
        private SerializedProperty propertyToggleChildren;
        private SerializedProperty propertyOpenDelay;
        private SerializedProperty propertyCloseDelay;

        private SerializedProperty propertyTransitions;

        private SerializedProperty propertyOnPreOpen;
        private SerializedProperty propertyOnOpen;
        private SerializedProperty propertyOnClose;
        private SerializedProperty propertyOnToggle;

        private SerializedProperty propertyExternalEnabled;
        private SerializedProperty propertyInputActionReferenceMultiplayer; 
                
        private bool editorFoldoutEvents;
        private bool editorFoldoutExternal;

        private void OnEnable()
        {
            propertyCloseOnStart = serializedObject.FindProperty("closeOnStart");
            propertyToggleChildren = serializedObject.FindProperty("toggleChildren");
            propertyOpenDelay = serializedObject.FindProperty("openDelay");
            propertyCloseDelay = serializedObject.FindProperty("closeDelay");

            propertyTransitions = serializedObject.FindProperty("transitions");

            propertyOnPreOpen = serializedObject.FindProperty("onPreOpen");
            propertyOnOpen = serializedObject.FindProperty("onOpen");
            propertyOnClose = serializedObject.FindProperty("onClose");
            propertyOnToggle = serializedObject.FindProperty("onToggle");

            propertyExternalEnabled = serializedObject.FindProperty("externalEnabled");
            propertyInputActionReferenceMultiplayer = serializedObject.FindProperty("inputActionReferenceMultiplayer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(propertyCloseOnStart);
            EditorGUILayout.PropertyField(propertyToggleChildren);
            EditorGUILayout.PropertyField(propertyOpenDelay);
            EditorGUILayout.PropertyField(propertyCloseDelay);

            EditorGUILayout.PropertyField(propertyTransitions);

            editorFoldoutEvents = EditorGUILayout.Foldout(editorFoldoutEvents, new GUIContent("Events"), true);
            if(editorFoldoutEvents)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(propertyOnPreOpen);
                EditorGUILayout.PropertyField(propertyOnOpen);
                EditorGUILayout.PropertyField(propertyOnClose);
                EditorGUILayout.PropertyField(propertyOnToggle);

                EditorGUI.indentLevel--;
            }

            editorFoldoutExternal = EditorGUILayout.Foldout(editorFoldoutExternal, new GUIContent("External"), true);
            if(editorFoldoutExternal)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(propertyExternalEnabled);
                EditorGUILayout.PropertyField(propertyInputActionReferenceMultiplayer);

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
