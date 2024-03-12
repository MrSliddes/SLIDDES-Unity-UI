using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Editor
{
    [CustomEditor(typeof(SideButton))]
    public class SideButtonEditor : ButtonSDSEditor
    {
        private SerializedProperty propertyDirection;
        private SerializedProperty propertyTextField;
        private SerializedProperty propertyCurrentIndex;
        private SerializedProperty propertySelectOnStart;
        private SerializedProperty propertyOptions;
        private SerializedProperty propertyOnSelectOption;

        public override void OnEnable()
        {
            base.OnEnable();
            propertyDirection = serializedObject.FindProperty("direction");
            propertyTextField = serializedObject.FindProperty("textField");
            propertyCurrentIndex = serializedObject.FindProperty("currentIndex");
            propertySelectOnStart = serializedObject.FindProperty("selectOnStart");
            propertyOptions = serializedObject.FindProperty("options");
            propertyOnSelectOption = serializedObject.FindProperty("onSelectOption");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.DrawInteractable();

            DrawValues();

            DrawTransitions();

            base.DrawNavigation();

            DrawEvents();

            DrawExternal();

            serializedObject.ApplyModifiedProperties();
        }

        public override void DrawValues()
        {
            EditorGUILayout.PropertyField(propertyDirection);
            EditorGUILayout.PropertyField(propertyTextField);
            EditorGUILayout.PropertyField(propertyCurrentIndex);
            EditorGUILayout.PropertyField(propertySelectOnStart);
            EditorGUILayout.PropertyField(propertyOptions);
            EditorGUILayout.PropertyField(propertyOnSelectOption);

            base.DrawValues();
        }


        [MenuItem("GameObject/UI/SLIDDES/Side Button")]
        public static void CreateSideButton()
        {
            GameObject prefab = Resources.Load("Side Button") as GameObject;
            if(prefab != null)
            {
                GameObject g = Instantiate(prefab);
                g.name = "Side Button";
                g.transform.SetParent(Selection.activeTransform, false);
            }
        }
    }
}
