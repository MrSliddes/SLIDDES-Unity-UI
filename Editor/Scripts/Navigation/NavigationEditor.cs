using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Editor
{
    [CustomEditor(typeof(Navigation))]
    public class NavigationEditor : UnityEditor.Editor
    {
        private SerializedProperty propertyInteractable;

        private SerializedProperty propertyNavigateUp;
        private SerializedProperty propertyNavigateDown;
        private SerializedProperty propertyNavigateLeft;
        private SerializedProperty propertyNavigateRight;

        private SerializedProperty propertySelectOnUp;
        private SerializedProperty propertySelectOnDown;
        private SerializedProperty propertySelectOnLeft;
        private SerializedProperty propertySelectOnRight;

        private bool editorFoldoutNavigate;

        public virtual void OnEnable()
        {
            propertyInteractable = serializedObject.FindProperty("interactable");

            propertyNavigateUp = serializedObject.FindProperty("navigateUp");
            propertyNavigateDown = serializedObject.FindProperty("navigateDown");
            propertyNavigateLeft = serializedObject.FindProperty("navigateLeft");
            propertyNavigateRight = serializedObject.FindProperty("navigateRight");

            propertySelectOnUp = serializedObject.FindProperty("selectOnUp");
            propertySelectOnDown = serializedObject.FindProperty("selectOnDown");
            propertySelectOnLeft = serializedObject.FindProperty("selectOnLeft");
            propertySelectOnRight = serializedObject.FindProperty("selectOnRight");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawInteractable();
            DrawNavigation();

            serializedObject.ApplyModifiedProperties();
        }

        public void DrawInteractable()
        {
            EditorGUILayout.PropertyField(propertyInteractable);
        }

        public void DrawNavigation()
        {
            editorFoldoutNavigate = EditorGUILayout.Foldout(editorFoldoutNavigate, new GUIContent("Navigation"), true);
            if(editorFoldoutNavigate)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = propertySelectOnUp.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateUp);
                GUI.enabled = propertyNavigateUp.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnUp);

                EditorGUILayout.Space();

                GUI.enabled = propertySelectOnDown.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateDown);
                GUI.enabled = propertyNavigateDown.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnDown);

                EditorGUILayout.Space();

                GUI.enabled = propertySelectOnLeft.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateLeft);
                GUI.enabled = propertyNavigateLeft.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnLeft);

                EditorGUILayout.Space();

                GUI.enabled = propertySelectOnRight.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateRight);
                GUI.enabled = propertyNavigateRight.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnRight);

                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }
    }
}
