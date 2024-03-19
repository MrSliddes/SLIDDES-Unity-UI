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
        private SerializedProperty propertyTimeBetweenPresses;

        private SerializedProperty propertyTransitionType;
        private SerializedProperty propertyTransitionProcedural;
        private SerializedProperty propertyTransitionColor;
        private SerializedProperty propertyTransitionSprite;
        private SerializedProperty propertyTransitionAnimation;
        private SerializedProperty propertyTransitionText;
        private SerializedProperty propertyTransitionRendering;
        private SerializedProperty propertyTransitionAudio;

        private SerializedProperty propertyInputAction;
        private SerializedProperty propertyInputPhaseCallbackIARM;
        private SerializedProperty propertyInputActionReferenceMultiplayer;

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
        private bool editorFoldoutExternal;

        public override void OnEnable()
        {
            base.OnEnable();
            propertyTimeBetweenPresses = serializedObject.FindProperty("timeBetweenPresses");

            propertyTransitionType = serializedObject.FindProperty("transitionType");
            propertyTransitionProcedural = serializedObject.FindProperty("transitionProcedural");
            propertyTransitionColor = serializedObject.FindProperty("transitionColor");
            propertyTransitionSprite = serializedObject.FindProperty("transitionSprite");
            propertyTransitionAnimation = serializedObject.FindProperty("transitionAnimation");
            propertyTransitionText = serializedObject.FindProperty("transitionText");
            propertyTransitionRendering = serializedObject.FindProperty("transitionRendering");
            propertyTransitionAudio = serializedObject.FindProperty("transitionAudio");

            propertyInputAction = serializedObject.FindProperty("inputAction");
            propertyInputPhaseCallbackIARM = serializedObject.FindProperty("inputPhaseCallbackIARM");
            propertyInputActionReferenceMultiplayer = serializedObject.FindProperty("inputActionReferenceMultiplayer");

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

            DrawValues();

            DrawTransitions();

            base.DrawNavigation();

            DrawEvents();

            DrawExternal();

            serializedObject.ApplyModifiedProperties();
        }

        public virtual void DrawValues()
        {
            EditorGUILayout.PropertyField(propertyTimeBetweenPresses);
        }

        public virtual void DrawTransitions()
        {
            // Transition
            EditorGUILayout.PropertyField(propertyTransitionType);
            EditorGUI.indentLevel++;
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.animation) == (int)Transition.Type.animation)
            {
                EditorGUILayout.PropertyField(propertyTransitionAnimation);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.audio) == (int)Transition.Type.audio)
            {
                EditorGUILayout.PropertyField(propertyTransitionAudio);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.color) == (int)Transition.Type.color)
            {
                EditorGUILayout.PropertyField(propertyTransitionColor);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.procedural) == (int)Transition.Type.procedural)
            {
                EditorGUILayout.PropertyField(propertyTransitionProcedural);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.rendering) == (int)Transition.Type.rendering)
            {
                EditorGUILayout.PropertyField(propertyTransitionRendering);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.sprite) == (int)Transition.Type.sprite)
            {
                EditorGUILayout.PropertyField(propertyTransitionSprite);
            }
            if((propertyTransitionType.enumValueFlag & (int)Transition.Type.text) == (int)Transition.Type.text)
            {
                EditorGUILayout.PropertyField(propertyTransitionText);
            }
            EditorGUI.indentLevel--;
        }

        public virtual void DrawEvents()
        {
            // Events
            editorFoldoutEvents = EditorGUILayout.Foldout(editorFoldoutEvents, new GUIContent("Events"), true);
            if(editorFoldoutEvents)
            {
                EditorGUI.indentLevel++;

                DrawEventsExtra();

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
        }

        public virtual void DrawEventsExtra()
        {

        }

        public virtual void DrawExternal()
        {
            // External
            editorFoldoutExternal = EditorGUILayout.Foldout(editorFoldoutExternal, new GUIContent("External"), true);
            if(editorFoldoutExternal)
            {
                EditorGUILayout.PropertyField(propertyInputAction);
                EditorGUILayout.PropertyField(propertyInputPhaseCallbackIARM);
                EditorGUILayout.PropertyField(propertyInputActionReferenceMultiplayer);
            }
        }


        [MenuItem("GameObject/UI/SLIDDES/ButtonSDS")]
        public static void CreateButtonSDS()
        {
            GameObject prefab = Resources.Load("ButtonSDS") as GameObject;
            if(prefab != null)
            {
                GameObject g = Instantiate(prefab);
                g.name = "ButtonSDS";
                g.transform.SetParent(Selection.activeTransform, false);
            }
        }
    }
}
