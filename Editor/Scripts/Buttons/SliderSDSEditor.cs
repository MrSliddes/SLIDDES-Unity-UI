using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Editor
{
    [CustomEditor(typeof(SliderSDS))]
    public class SliderSDSEditor : ButtonSDSEditor
    {
        private SerializedProperty propertyDirection;
        private SerializedProperty propertyMinValue;
        private SerializedProperty propertyMaxValue;
        private SerializedProperty propertyValue;
        private SerializedProperty propertyValueIsInt;
        private SerializedProperty propertyStepSize;

        private SerializedProperty propertyFiller;
        private SerializedProperty propertyHandle;

        private SerializedProperty propertyTriggerValueChangedOnStart;
        private SerializedProperty propertyOnValueChanged;
        private SerializedProperty propertyOnNormalizedValueChanged;

        public override void OnEnable()
        {
            base.OnEnable();
            propertyDirection = serializedObject.FindProperty("direction");
            propertyMinValue = serializedObject.FindProperty("minValue");
            propertyMaxValue = serializedObject.FindProperty("maxValue");
            propertyValue = serializedObject.FindProperty("value");
            propertyValueIsInt = serializedObject.FindProperty("valueIsInt");
            propertyStepSize = serializedObject.FindProperty("stepSize");

            propertyFiller = serializedObject.FindProperty("filler");
            propertyHandle = serializedObject.FindProperty("handle");

            propertyTriggerValueChangedOnStart = serializedObject.FindProperty("triggerValueChangedOnStart");
            propertyOnValueChanged = serializedObject.FindProperty("onValueChanged");
            propertyOnNormalizedValueChanged = serializedObject.FindProperty("onNormalizedValueChanged");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.DrawInteractable();

            DrawSliderValues();

            DrawTransitions();

            base.DrawNavigation();

            DrawEvents();

            DrawExternal();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawSliderValues()
        {
            EditorGUILayout.PropertyField(propertyDirection);
            EditorGUILayout.PropertyField(propertyMinValue);
            EditorGUILayout.PropertyField(propertyMaxValue);
            if(propertyValueIsInt.boolValue)
            {
                propertyValue.floatValue = EditorGUILayout.IntSlider(new GUIContent("Value"), (int)propertyValue.floatValue, (int)propertyMinValue.floatValue, (int)propertyMaxValue.floatValue);
            }
            else
            {
                propertyValue.floatValue = EditorGUILayout.Slider(new GUIContent("Value"), propertyValue.floatValue, propertyMinValue.floatValue, propertyMaxValue.floatValue);
            }
            EditorGUILayout.PropertyField(propertyValueIsInt);
            EditorGUILayout.PropertyField(propertyStepSize);

            EditorGUILayout.PropertyField(propertyFiller);
            EditorGUILayout.PropertyField(propertyHandle);
        }

        public override void DrawEventsExtra()
        {
            EditorGUILayout.PropertyField(propertyTriggerValueChangedOnStart);
            EditorGUILayout.PropertyField(propertyOnValueChanged);
            EditorGUILayout.PropertyField(propertyOnNormalizedValueChanged);

            base.DrawEventsExtra();
        }

        [MenuItem("GameObject/UI/SLIDDES/SliderSDS")]
        public static void CreateSliderSDS()
        {
            GameObject prefab = Resources.Load("SliderSDS") as GameObject;
            if(prefab != null)
            {
                GameObject g = Instantiate(prefab);
                g.name = "SliderSDS";
                g.transform.SetParent(Selection.activeTransform, false);
            }
        }
    }
}
