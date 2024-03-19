using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    /// <summary>
    /// Monobehaviour wrapper class for InputActionReferenceMultiplayer
    /// </summary>
    public class InputActionReferenceMultiplayerHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReferenceMultiplayer iarm;

        public UnityEvent<InputAction.CallbackContext> onCallback;
        public UnityEvent<InputAction.CallbackContext> onCallbackCanceled;
        public UnityEvent<InputAction.CallbackContext> onCallbackPerformed;

        private void Awake()
        {
            iarm.Callback = x =>
            {
                onCallback?.Invoke(x);
                if(x.canceled)
                {
                    onCallbackCanceled?.Invoke(x);
                }
                else if(x.performed)
                {
                    onCallbackPerformed?.Invoke(x);
                }
            };
        }

        private void OnEnable()
        {
            iarm.Enable();
        }

        private void OnDisable()
        {
            iarm.Disable();
        }
    }
}
