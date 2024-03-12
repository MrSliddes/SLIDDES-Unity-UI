using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class FloatToText : MonoBehaviour
    {
        [SerializeField] private string format;
        private TMP_Text textField;

        private void Awake()
        {
            textField = GetComponent<TMP_Text>();
        }

        public void Set(float value)
        {
            textField.text = value.ToString(format);
        }
    }
}
