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

        private SerializedProperty propertyMode;

        private SerializedProperty propertyNavigateUp;
        private SerializedProperty propertyNavigateDown;
        private SerializedProperty propertyNavigateLeft;
        private SerializedProperty propertyNavigateRight;

        private SerializedProperty propertySelectOnUp;
        private SerializedProperty propertySelectOnDown;
        private SerializedProperty propertySelectOnLeft;
        private SerializedProperty propertySelectOnRight;

        private SerializedProperty propertyNavigateUpTriggerSubmit;
        private SerializedProperty propertyNavigateDownTriggerSubmit;
        private SerializedProperty propertyNavigateLeftTriggerSubmit;
        private SerializedProperty propertyNavigateRightTriggerSubmit;

        private SerializedProperty propertySelectFirstFromGroup;
        private SerializedProperty propertyGroup;

        private bool editorFoldoutNavigate;

        public virtual void OnEnable()
        {
            propertyInteractable = serializedObject.FindProperty("interactable");

            propertyMode = serializedObject.FindProperty("mode");

            propertyNavigateUp = serializedObject.FindProperty("navigateUp");
            propertyNavigateDown = serializedObject.FindProperty("navigateDown");
            propertyNavigateLeft = serializedObject.FindProperty("navigateLeft");
            propertyNavigateRight = serializedObject.FindProperty("navigateRight");

            propertySelectOnUp = serializedObject.FindProperty("selectOnUp");
            propertySelectOnDown = serializedObject.FindProperty("selectOnDown");
            propertySelectOnLeft = serializedObject.FindProperty("selectOnLeft");
            propertySelectOnRight = serializedObject.FindProperty("selectOnRight");

            propertyNavigateUpTriggerSubmit = serializedObject.FindProperty("navigateUpTriggerSubmit");
            propertyNavigateDownTriggerSubmit = serializedObject.FindProperty("navigateDownTriggerSubmit");
            propertyNavigateLeftTriggerSubmit = serializedObject.FindProperty("navigateLeftTriggerSubmit");
            propertyNavigateRightTriggerSubmit = serializedObject.FindProperty("navigateRightTriggerSubmit");

            propertySelectFirstFromGroup = serializedObject.FindProperty("selectFirstFromGroup");
            propertyGroup = serializedObject.FindProperty("group");
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

                EditorGUILayout.PropertyField(propertyMode);

                GUI.enabled = propertySelectOnUp.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateUp);
                GUI.enabled = propertyNavigateUp.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnUp);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(propertyNavigateUpTriggerSubmit);

                EditorGUILayout.Space();

                GUI.enabled = propertySelectOnDown.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateDown);
                GUI.enabled = propertyNavigateDown.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnDown);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(propertyNavigateDownTriggerSubmit);

                EditorGUILayout.Space();

                GUI.enabled = propertySelectOnLeft.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateLeft);
                GUI.enabled = propertyNavigateLeft.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnLeft);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(propertyNavigateLeftTriggerSubmit);

                EditorGUILayout.Space();

                GUI.enabled = propertySelectOnRight.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertyNavigateRight);
                GUI.enabled = propertyNavigateRight.objectReferenceValue == null;
                EditorGUILayout.PropertyField(propertySelectOnRight);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(propertyNavigateRightTriggerSubmit);

                GUI.enabled = true;

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(propertySelectFirstFromGroup);
                EditorGUILayout.PropertyField(propertyGroup);
                if(GUILayout.Button(new GUIContent("Auto Assign Group")))
                {
                    ((Navigation)target).AutoAssignGroup();
                    EditorUtility.SetDirty(this);
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}
