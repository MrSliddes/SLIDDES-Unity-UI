using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Buttons/InputFieldHandler")]
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldHandler : MonoBehaviour
    {
        [SerializeField] private float minValue = 0;
        [SerializeField] private float maxValue = 100;
        [SerializeField] private string format = "0";

        public UnityEvent<float> onValueChanged;

        private TMP_InputField inputField;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();            
        }

        private void OnEnable()
        {
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            inputField.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(string text)
        {
            if(float.TryParse(text, out float textValue))
            {
                textValue = Mathf.Clamp(textValue, minValue, maxValue);
                inputField.SetTextWithoutNotify(textValue.ToString(format));
                onValueChanged.Invoke(textValue);
            }
        }
    }
}
