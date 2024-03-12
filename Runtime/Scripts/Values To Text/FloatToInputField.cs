using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class FloatToInputField : MonoBehaviour
    {
        [SerializeField] private string format;
        private TMP_InputField inputField;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void Set(float value)
        {
            inputField.text = value.ToString(format);
        }

        public void SetNoNotify(float value)
        {
            inputField.SetTextWithoutNotify(value.ToString(format));
        }
    }
}
