using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class InputPromptText : MonoBehaviour
    {
        public string Text
        { 
            get
            {
                return text;
            }
            set
            {
                text = value;
                ConvertTextToPrompt();
            }
        }

        [TextArea(1, 10)]
        [SerializeField] private string text = "<input=[0]>";
        [SerializeField] private InputActionReference[] inputActionReferences;

        private TMP_Text textField;

        private void Awake()
        {
            textField = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            InputManager.OnInputDeviceNameChanged += x => ConvertTextToPrompt();
            InputManager.OnForceUpdateInputPrompts += () => ConvertTextToPrompt();
            InputManager.OnLastPlayerInputPressChanged += x => ConvertTextToPrompt();
        }

        private void OnDisable()
        {
            InputManager.OnInputDeviceNameChanged += x => ConvertTextToPrompt();
            InputManager.OnForceUpdateInputPrompts -= () => ConvertTextToPrompt();
            InputManager.OnLastPlayerInputPressChanged -= x => ConvertTextToPrompt();
        }

        private void OnValidate()
        {
            if(textField == null) textField = GetComponent<TMP_Text>();
            ConvertTextToPrompt();
        }

        // Start is called before the first frame update
        void Start()
        {
            ConvertTextToPrompt();
        }

        public void ConvertTextToPrompt()
        {
            if(inputActionReferences.Length <= 0) return;

            textField.text = text;

            // Look for text that is equal to "<input=[0]>"
            // The 0 is the index of the inputActionReference
            string pattern = @"<input=\[.*?\]>";
            MatchCollection matchCollection = Regex.Matches(text, pattern);
            // For every match
            foreach(Match match in matchCollection)
            {
                // Get the number between [ ]
                string value = match.Value;
                value = value.Replace("<input=[", "").Replace("]>", "");
                if(int.TryParse(value, out int index))
                {
                    // Get the input action reference from the array based on the index
                    InputActionReference inputActionReference = inputActionReferences[index];

                    // Replace the match value with the generated prompt
                    string prompt = InputManager.GetInputPrompt(inputActionReference);
                    textField.text = textField.text.Replace(match.Value, prompt);                    
                }
            }
        }
    }
}
