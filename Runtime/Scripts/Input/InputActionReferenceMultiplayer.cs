using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class InputActionReferenceMultiplayer
    {
        public string ActionName => inputActionReference.action.name;
        public System.Action<InputAction.CallbackContext> Callback
        {
            get
            {
                return callback;
            }
            set
            {
                callback = value;
            }
        }

        [SerializeField] private InputActionReference inputActionReference;

        private System.Action<InputAction.CallbackContext> callback;

        public void Enable()
        {
            if(inputActionReference == null) return;

            InputManager.AddInputActionReferenceMultiplayer(this);
        }

        public void Disable()
        {
            if(inputActionReference == null) return;

            InputManager.RemoveInputActionReferenceMultiplayer(this);
        }

        public void AddCallbackToAction(InputAction inputAction)
        {
            inputAction.started += Callback;
            inputAction.performed += Callback;
            inputAction.canceled += Callback;
        }

        public void RemoveCallbackFromAction(InputAction inputAction)
        {
            inputAction.started -= Callback;
            inputAction.performed -= Callback;
            inputAction.canceled -= Callback;
        }
    }
}
