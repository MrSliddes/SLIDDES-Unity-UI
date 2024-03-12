using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class InputSystemIAR
    {
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

        public Type type;

        private System.Action<InputAction.CallbackContext> callback;

        public void Enable()
        {
            InputSystemUIInputModule module = GetInputSystemUIInputModule();
            if(module == null) return;

            switch(type)
            {
                case Type.none:
                    break;
                case Type.point:
                    module.point.action.canceled += Callback;
                    break;
                case Type.leftClick:
                    module.leftClick.action.canceled += Callback;
                    break;
                case Type.middleClick:
                    module.middleClick.action.canceled += Callback;
                    break;
                case Type.rightClick:
                    module.rightClick.action.canceled += Callback;
                    break;
                case Type.scrollWheel:
                    module.scrollWheel.action.canceled += Callback;
                    break;
                case Type.move:
                    module.move.action.canceled += Callback;
                    break;
                case Type.submit:
                    module.submit.action.canceled += Callback;
                    break;
                case Type.cancel:
                    module.cancel.action.canceled += Callback;
                    break;
                default:
                    break;
            }
        }

        public void Disable()
        {
            InputSystemUIInputModule module = GetInputSystemUIInputModule();
            if(module == null) return;

            switch(type)
            {
                case Type.none:
                    break;
                case Type.point:
                    module.point.action.canceled -= Callback;
                    break;
                case Type.leftClick:
                    module.leftClick.action.canceled -= Callback;
                    break;
                case Type.middleClick:
                    module.middleClick.action.canceled -= Callback;
                    break;
                case Type.rightClick:
                    module.rightClick.action.canceled -= Callback;
                    break;
                case Type.scrollWheel:
                    module.scrollWheel.action.canceled -= Callback;
                    break;
                case Type.move:
                    module.move.action.canceled -= Callback;
                    break;
                case Type.submit:
                    module.submit.action.canceled -= Callback;
                    break;
                case Type.cancel:
                    module.cancel.action.canceled -= Callback;
                    break;
                default:
                    break;
            }
        }

        private InputSystemUIInputModule GetInputSystemUIInputModule()
        {
            if(EventSystem.current == null)
            {
                return null;
            }

            if(EventSystem.current.currentInputModule == null)
            {
                return EventSystem.current.GetComponent<InputSystemUIInputModule>();
            }

            return ((InputSystemUIInputModule)EventSystem.current.currentInputModule);
        }

        [System.Serializable]
        public enum Type
        {
            none,
            point,
            leftClick,
            middleClick,
            rightClick,
            scrollWheel,
            move,
            submit,
            cancel
        }
    }
}
