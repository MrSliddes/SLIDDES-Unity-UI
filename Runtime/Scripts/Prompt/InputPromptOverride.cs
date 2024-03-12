using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI
{
    /// <summary>
    /// When a certain SpriteAsset is selected, override the text
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class InputPromptOverride : MonoBehaviour, IInputPrompt
    {        
        [SerializeField] public Override[] overrides;

        private string defaultText;
        private TMP_Text textField;

        private void Awake()
        {
            textField = GetComponent<TMP_Text>();
            defaultText = textField.text;            
        }

        void IInputPrompt.OnSpriteAssetChange(string currentInputDeviceProfileLabel)
        {
            Override @override = overrides.FirstOrDefault(x => x.label == currentInputDeviceProfileLabel);
            if(@override != null)
            {
                textField.text = @override.text;
            }
            else
            {
                textField.text = defaultText;
            }
        }

        [System.Serializable]
        public class Override
        {
            public string label;
            [TextArea(1, 10)]
            public string text;
        }
    }
}
