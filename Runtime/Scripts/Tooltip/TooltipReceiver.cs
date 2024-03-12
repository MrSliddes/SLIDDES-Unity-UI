using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    public class TooltipReceiver : MonoBehaviour
    {
        [SerializeField] private bool noTextIsHide;
        [SerializeField] private float fadeTime = 0.1f;
        [SerializeField] private GameObject container;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color backgroundColorDefault = Color.white;
        [SerializeField] private Color backgroundColorShow = Color.black;
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Color textFieldColorDefault = Color.black;
        [SerializeField] private Color textFieldColorShow = Color.white;

        private void OnEnable()
        {
            if(backgroundImage != null)
            {
                // Using crossfade so this needs to be white
                backgroundImage.color = Color.white;
            }

            Receive(null);
        }

        private void Start()
        {
            if(backgroundImage != null)
            {
                // Using crossfade so this needs to be white
                backgroundImage.color = Color.white;
            }

            Receive(null);
        }

        public void Receive(TooltipContent content)
        {
            if(content == null || (noTextIsHide && string.IsNullOrEmpty(content.description)))
            {
                // Hide
                if(container != null) container.SetActive(false);
                if(textField != null)
                {
                    textField.text = "";
                    textField.CrossFadeColor(textFieldColorDefault, fadeTime, true, true);
                }
                if(backgroundImage != null)
                {
                    backgroundImage.CrossFadeColor(backgroundColorDefault, fadeTime, true, true);
                }
                return;
            }

            // Show
            if(textField != null)
            {
                textField.text = content.description;
                textField.CrossFadeColor(textFieldColorShow, fadeTime, true, true);
            }
            if(backgroundImage != null)
            {
                backgroundImage.CrossFadeColor(backgroundColorShow, fadeTime, true, true);
            }
            if(container != null) container.SetActive(true);
        }
    }
}
