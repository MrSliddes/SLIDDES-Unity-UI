using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace SLIDDES.UI.Menus
{
    [AddComponentMenu("SLIDDES/UI/Menus/Menu")]
    public class Menu : MonoBehaviour
    {
        /// <summary>
        /// Close the menu on start
        /// </summary>
        public bool CloseOnStart => closeOnStart;
        /// <summary>
        /// Is the menu closed?
        /// </summary>
        public bool IsClosed => isClosed;
        /// <summary>
        /// Are the externals enabled?
        /// </summary>
        public bool ExternalEnabled
        {
            get
            {
                return externalEnabled;
            }
            set
            {
                externalEnabled = value;
            }
        }

        [Tooltip("Close this menu on start")]
        [SerializeField] private bool closeOnStart;
        [Tooltip("When this menu gets opend all children are set active and vice versa")]
        [SerializeField] private bool toggleChildren;
        [Tooltip("Delay opening the menu when Open() is called")]
        [SerializeField] private float openDelay;
        [Tooltip("Delaying closing the menu when Close() is called")]
        [SerializeField] private float closeDelay;

        [SerializeField] private Transitions transitions;

        [Tooltip("Triggerd when the menu opens, called before onOpen")]
        public UnityEvent onPreOpen;
        [Tooltip("Triggerd when the menu opens")]
        public UnityEvent onOpen;
        [Tooltip("Triggerd when the menu closes, called before onClose")]
        public UnityEvent onPreClose;
        [Tooltip("Triggerd when the menu closes")]
        public UnityEvent onClose;
        [Tooltip("Triggerd when the menu opens/closes. bool = isClosed")]
        public UnityEvent<bool> onToggle;

        [SerializeField] private bool externalEnabled = true;
        [SerializeField] private InputActionReferenceMultiplayer inputActionReferenceMultiplayer;

        /// <summary>
        /// Is the menu closed
        /// </summary>
        private bool isClosed;
        private float gameSpeedBeforePause;
        private Coroutine coroutineOpen;
        private Coroutine coroutineClose;

        private void Awake()
        {
            inputActionReferenceMultiplayer.Callback = x =>
            {
                if(externalEnabled)
                {
                    Toggle();
                }
            };
        }

        private void OnEnable()
        {
            inputActionReferenceMultiplayer.Enable();
            transitions.OnEnable();
        }

        private void OnDisable()
        {
            inputActionReferenceMultiplayer.Disable();
            transitions.OnDisable();
        }

        // Start is called before the first frame update
        void Start()
        {
            if(closeOnStart)
            {
                Close();
            }
        }

        /// <summary>
        /// Close the menu
        /// </summary>
        public void Close()
        {
            if(isActiveAndEnabled)
            {
                if(coroutineOpen != null) StopCoroutine(coroutineOpen);

                if(coroutineClose != null) StopCoroutine(coroutineClose);
                coroutineClose = StartCoroutine(CloseAsync());
            }
        }

        /// <summary>
        /// Open the menu
        /// </summary>
        public void Open()
        {
            if(isActiveAndEnabled)
            { 
                if(coroutineClose != null) StopCoroutine(coroutineClose);

                if(coroutineOpen != null) StopCoroutine(coroutineOpen);
                coroutineOpen = StartCoroutine(OpenAsync());                        
            }
        }

        /// <summary>
        /// Toggle the menu to be open/closed
        /// </summary>
        public void Toggle()
        {
            if(IsClosed)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Toggle(bool value)
        {
            if(value)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Toggle children of transform
        /// </summary>
        private void ToggleChildren()
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(!IsClosed);
            }
        }

        private IEnumerator OpenAsync()
        {
            onPreOpen?.Invoke();

            if(openDelay > 0) yield return new WaitForSecondsRealtime(openDelay);

            isClosed = false;
            if(toggleChildren) ToggleChildren();
            onOpen?.Invoke();
            onToggle?.Invoke(true);

            yield break;
        }

        private IEnumerator CloseAsync()
        {
            onPreClose?.Invoke();

            if(closeDelay > 0) yield return new WaitForSecondsRealtime(closeDelay);

            isClosed = true;
            if(toggleChildren) ToggleChildren();
            onClose?.Invoke();
            onToggle?.Invoke(false);

            yield break;
        }
    }
}
