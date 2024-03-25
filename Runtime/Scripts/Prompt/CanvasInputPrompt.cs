using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    /// <summary>
    /// Updates input prompts on this canvas
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/CanvasInputPrompt")]
    [RequireComponent(typeof(Canvas))]
    public class CanvasInputPrompt : MonoBehaviour
    {
        private UnityAction<string> actionOnInputDeviceNameChanged;
        private UnityAction actionOnForceUpdatePrompts;
        private UnityAction<InputManager.Player> actionOnLastPlayerInputPressChanged;

        private void Awake()
        {
            actionOnInputDeviceNameChanged = x =>
            {
                if(!string.IsNullOrEmpty(x))
                {
                    UpdatePrompts();
                }
            };
            actionOnForceUpdatePrompts = () =>
            {
                UpdatePrompts();
            };
            actionOnLastPlayerInputPressChanged = x =>
            {
                if(x != null)
                {
                    UpdatePrompts();
                }
            };
        }

        private void OnEnable()
        {
            if(InputManager.Instance != null)
            {
                InputManager.OnInputDeviceNameChanged += actionOnInputDeviceNameChanged;
                InputManager.OnForceUpdateInputPrompts += actionOnForceUpdatePrompts;
                InputManager.OnLastPlayerInputPressChanged += actionOnLastPlayerInputPressChanged;
                UpdatePrompts();
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.OnInputDeviceNameChanged -= actionOnInputDeviceNameChanged;
                InputManager.OnForceUpdateInputPrompts -= actionOnForceUpdatePrompts;
                InputManager.OnLastPlayerInputPressChanged -= actionOnLastPlayerInputPressChanged;                
            }
        }

        private void OnDestroy()
        {
            if(InputManager.Instance != null)
            {
                InputManager.OnInputDeviceNameChanged -= actionOnInputDeviceNameChanged;
                InputManager.OnForceUpdateInputPrompts -= actionOnForceUpdatePrompts;
                InputManager.OnLastPlayerInputPressChanged -= actionOnLastPlayerInputPressChanged;
            }
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
