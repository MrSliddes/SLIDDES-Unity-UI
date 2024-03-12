using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI
{
    /// <summary>
    /// Updates input prompts on this canvas
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/CanvasInputPrompt")]
    [RequireComponent(typeof(Canvas))]
    public class CanvasInputPrompt : MonoBehaviour
    {
        private void OnEnable()
        {
            InputManager.OnInputDeviceNameChanged += x => UpdatePrompts();
            InputManager.OnForceUpdateInputPrompts += () => UpdatePrompts();
            InputManager.OnLastPlayerInputPressChanged += x => UpdatePrompts();
            UpdatePrompts();
        }

        private void OnDisable()
        {
            InputManager.OnInputDeviceNameChanged -= x => UpdatePrompts();
            InputManager.OnForceUpdateInputPrompts -= () => UpdatePrompts();
            InputManager.OnLastPlayerInputPressChanged -= x => UpdatePrompts();
        }

        // Start is called before the first frame update
        void Start()
        {
            UpdatePrompts();
        }

        public void UpdatePrompts()
        {
            if(InputManager.Instance == null)
            {
                Debug.LogWarning("[CanvasInputPrompt] No InputManager found");
                return;
            }

            TMP_Text[] texts = transform.GetComponentsInChildren<TMP_Text>(true);
            for(int j = 0; j < texts.Length; j++)
            {
                IInputPrompt inputPrompt = texts[j].GetComponent<IInputPrompt>();
                if(inputPrompt != null && inputPrompt.IgnoreByManager()) continue;

                texts[j].spriteAsset = InputManager.CurrentSpriteAsset;
                if(inputPrompt != null)
                {
                    inputPrompt.OnSpriteAssetChange(InputManager.CurrentSpriteAsset);
                    inputPrompt.OnSpriteAssetChange(InputManager.CurrentInputDeviceProfile.label);
                }
            }
        }
    }
}
