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

        public UnityEvent<InputAction.CallbackContext> onCallbackStarted;
        public UnityEvent<InputAction.CallbackContext> onCallbackPerformed;
        public UnityEvent<InputAction.CallbackContext> onCallbackCanceled;

        private void Awake()
        {
            iarm.Callback = x =>
            {
                if(x.started)
                {
                    onCallbackStarted?.Invoke(x);
                }
                if(x.performed)
                {
                    onCallbackPerformed?.Invoke(x);
                }
                if(x.canceled)
                {
                    onCallbackCanceled?.Invoke(x);
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

        private void OnDestroy()
        {
            iarm.Disable();
        }
    }
}
