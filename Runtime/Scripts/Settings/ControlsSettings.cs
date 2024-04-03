using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    public class ControlsSettings : MonoBehaviour
    {
        [SerializeField] private bool loadOnAwake;
        [SerializeField] private bool applyOnAwake;

        [Header("Defaults")]
        [SerializeField] private int defaultCursorVisibility;

        [Space]

        [SerializeField] private bool showDebug;

        public UnityEvent<int> onGetCursorVisibility;

        private readonly string savePrefix = "ControlsSettings";
        private readonly string debugPrefix = "[ControlsSettings]";

        private CursorVisibility cursorVisibility;

        private void Awake()
        {
            if(loadOnAwake) LoadSettings();
            if(applyOnAwake) ApplySettings();
        }

        public void LoadSettings()
        {
            // Hide mouse on controller input
            cursorVisibility = (CursorVisibility)PlayerPrefs.GetInt(GetPlayerPrefKey("cursorVisibility"), defaultCursorVisibility);
            onGetCursorVisibility?.Invoke((int)cursorVisibility);
            
            if(showDebug) Debug.Log($"{debugPrefix} Loaded Controls Settings");
        }

        public void ApplySettings()
        {
            ApplyCursorVisibility();

            if(showDebug) Debug.Log($"{debugPrefix} Applied Controls Settings");
        }

        public void SetCursorVisibility(int value)
        {
            cursorVisibility = (CursorVisibility)value;            
        }

        public void ApplyCursorVisibility()
        {
            switch(cursorVisibility)
            {
                case CursorVisibility.HideOnControllerInput:
                    InputManager.HideCursorOnControllerInput = true;
                    if(InputManager.Instance != null)
                    {
                        if(!InputManager.CurrentInputDeviceIsMouseAndKeyboard)
                        {
                            Cursor.visible = false;
                        }
                    }
                    break;
                case CursorVisibility.Show:
                    InputManager.HideCursorOnControllerInput = false;
                    Cursor.visible = true;
                    break;
                default:
                    break;
            }
            PlayerPrefs.SetInt(GetPlayerPrefKey("cursorVisibility"), (int)cursorVisibility);
        }

        private string GetPlayerPrefKey(string key)
        {
            return $"{Application.productName}_{savePrefix}_{key}";
        }

        public enum CursorVisibility
        { 
            HideOnControllerInput = 0,
            Show = 1
        }
    }
}
