using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace SLIDDES.UI.Editor
{
    [CustomEditor(typeof(ButtonSDS))]
    public class ButtonSDSEditor : NavigationEditor
    {
        private SerializedProperty propertyTransitionType;
        private SerializedProperty propertyTransitionProcedural;
        private SerializedProperty propertyTransitionColor;
        private SerializedProperty propertyTransitionSprite;
        private SerializedProperty propertyTransitionAnimation;
        private SerializedProperty propertyTransitionText;

        private SerializedProperty propertyInputActionExternalSubmit;

        private SerializedProperty propertyOnMove;
        private SerializedProperty propertyOnClick;
        private SerializedProperty propertyOnPointerDown;
        private SerializedProperty propertyOnPointerUp;
        private SerializedProperty propertyOnPointerEnter;
        private SerializedProperty propertyOnPointerExit;
        private SerializedProperty propertyOnSelect;
        private SerializedProperty propertyOnDeselect;
        private SerializedProperty propertyOnHoverEnter;
        private SerializedProperty propertyOnHoverExit;

        private SerializedProperty propertyEventDelayOnClick;
        private SerializedProperty propertyEventDelayOnPointerDown;
        private SerializedProperty propertyEventDelayOnPointerUp;
        private SerializedProperty propertyEventDelayOnPointerEnter;
        private SerializedProperty propertyEventDelayOnPointerExit;
        private SerializedProperty propertyEventDelayOnSelect;
        private SerializedProperty propertyEventDelayOnDeselect;
        private SerializedProperty propertyEventDelayOnHoverEnter;
        private SerializedProperty propertyEventDelayOnHoverExit;

        private bool editorFoldoutEvents;

        public override void OnEnable()
        {
            base.OnEnable();
            propertyTransitionType = serializedObject.FindProperty("transitionType");
            propertyTransitionProcedural = serializedObject.FindProperty("transitionProcedural");
            propertyTransitionColor = serializedObject.FindProperty("transitionColor");
            propertyTransitionSprite = serializedObject.FindProperty("transitionSprite");
            propertyTransitionAnimation = serializedObject.FindProperty("transitionAnimation");
            propertyTransitionText = serializedObject.FindProperty("transitionText");

            propertyInputActionExternalSubmit = serializedObject.FindProperty("inputActionExternalSubmit");

            propertyOnMove = serializedObject.FindProperty("onMove");
            propertyOnClick = serializedObject.FindProperty("onClick");
            propertyOnPointerDown = serializedObject.FindProperty("onPointerDown");
            propertyOnPointerUp = serializedObject.FindProperty("onPointerUp");
            propertyOnPointerEnter = serializedObject.FindProperty("onPointerEnter");
            propertyOnPointerExit = serializedObject.FindProperty("onPointerExit");
            propertyOnSelect = serializedObject.FindProperty("onSelect");
            propertyOnDeselect = serializedObject.FindProperty("onDeselect");
            propertyOnHoverEnter = serializedObject.FindProperty("onHoverEnter");
            propertyOnHoverExit = serializedObject.FindProperty("onHoverExit");

            propertyEventDelayOnClick = serializedObject.FindProperty("eventDelayOnClick");
            propertyEventDelayOnPointerDown = serializedObject.FindProperty("eventDelayOnPointerDown");
            propertyEventDelayOnPointerUp = serializedObject.FindProperty("eventDelayOnPointerUp");
            propertyEventDelayOnPointerEnter = serializedObject.FindProperty("eventDelayOnPointerEnter");
            propertyEventDelayOnPointerExit = serializedObject.FindProperty("eventDelayOnPointerExit");
            propertyEventDelayOnSelect = serializedObject.FindProperty("eventDelayOnSelect");
            propertyEventDelayOnDeselect = serializedObject.FindProperty("eventDelayOnDeselect");
            propertyEventDelayOnHoverEnter = serializedObject.FindProperty("eventDelayOnHoverEnter");
            propertyEventDelayOnHoverExit = serializedObject.FindProperty("eventDelayOnHoverExit");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.DrawInteractable();

            // Transition
            EditorGUILayout.PropertyField(propertyTransitionType);
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.procedural) == (int)Transition.Type.procedural)
            {
                EditorGUILayout.PropertyField(propertyTransitionProcedural);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.color) == (int)Transition.Type.color)
            {
                EditorGUILayout.PropertyField(propertyTransitionColor);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.sprite) == (int)Transition.Type.sprite)
            {
                EditorGUILayout.PropertyField(propertyTransitionSprite);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.animation) == (int)Transition.Type.animation)
            {
                EditorGUILayout.PropertyField(propertyTransitionAnimation);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.text) == (int)Transition.Type.text)
            {
                EditorGUILayout.PropertyField(propertyTransitionText);
            }

            base.DrawNavigation();

            // Events
            editorFoldoutEvents = EditorGUILayout.Foldout(editorFoldoutEvents, new GUIContent("Events"), true);
            if(editorFoldoutEvents)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(propertyOnMove);

                EditorGUILayout.PropertyField(propertyEventDelayOnClick);
                EditorGUILayout.PropertyField(propertyOnClick);

                EditorGUILayout.PropertyField(propertyEventDelayOnPointerDown);
                EditorGUILayout.PropertyField(propertyOnPointerDown);

                EditorGUILayout.PropertyField(propertyEventDelayOnPointerUp);
                EditorGUILayout.PropertyField(propertyOnPointerUp);

                EditorGUILayout.PropertyField(propertyEventDelayOnPointerEnter);
                EditorGUILayout.PropertyField(propertyOnPointerEnter);

                EditorGUILayout.PropertyField(propertyEventDelayOnPointerExit);
                EditorGUILayout.PropertyField(propertyOnPointerExit);

                EditorGUILayout.PropertyField(propertyEventDelayOnSelect);
                EditorGUILayout.PropertyField(propertyOnSelect);

                EditorGUILayout.PropertyField(propertyEventDelayOnDeselect);
                EditorGUILayout.PropertyField(propertyOnDeselect);

                EditorGUILayout.PropertyField(propertyEventDelayOnHoverEnter);
                EditorGUILayout.PropertyField(propertyOnHoverEnter);

                EditorGUILayout.PropertyField(propertyEventDelayOnHoverExit);
                EditorGUILayout.PropertyField(propertyOnHoverExit);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(propertyInputActionExternalSubmit, new GUIContent("External Submit"));

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("GameObject/UI/SLIDDES/ButtonSDS")]
        public static void CreateButton()
        {
            GameObject g = new GameObject("ButtonSDS");
            g.transform.SetParent(Selection.activeTransform);
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = Vector3.zero;
            g.AddComponent<RectTransform>();
            g.AddComponent<CanvasRenderer>();
            ButtonSDS button = g.AddComponent<ButtonSDS>();
            button.TransitionProcedural = new TransitionProcedural();
            button.TransitionType |= Transition.Type.color;
            button.TransitionColor = new TransitionColor();

            GameObject targetGraphic = new GameObject("Target Graphic");
            targetGraphic.transform.SetParent(g.transform);
            targetGraphic.transform.localScale = Vector3.one;
            targetGraphic.transform.localPosition = Vector3.zero;
            targetGraphic.AddComponent<CanvasRenderer>();
            Image image = targetGraphic.AddComponent<Image>();
            button.TransitionProcedural.TargetGraphic = image;

            GameObject tmp = new GameObject("Text (TMP)");
            tmp.transform.SetParent(targetGraphic.transform);
            tmp.transform.localScale = Vector3.one;
            tmp.transform.localPosition = Vector3.zero;
            tmp.AddComponent<CanvasRenderer>();
            TextMeshProUGUI textField = tmp.AddComponent<TextMeshProUGUI>();
            textField.rectTransform.anchorMin = new Vector2(0, 0);
            textField.rectTransform.anchorMax = new Vector2(1, 1);
            textField.rectTransform.SetRect(0, 0, 0, 0);
            textField.text = "Button";
            textField.color = Color.black;
            textField.enableAutoSizing = true;
            textField.alignment = TextAlignmentOptions.Center;
            textField.margin = new Vector4(8, 0, 8, 0);
            button.TransitionColor.TargetGraphic = image;
            button.TransitionColor.TextField = textField;
        }
    }
}
