using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// Base class for navigator menus
    /// </summary>
    public abstract class Navigator : MonoBehaviour
    {
        [Tooltip("The way this menu closes on application start")]
        [SerializeField] public InitializationMode initializationMode;

        [Tooltip("If set true, the first index children get setActive(menu on/off)")]
        [SerializeField] private bool toggleChildren = true;

        [Header("Callbacks")]
        public UnityEvent onMenuOpen;
        public UnityEvent onMenuClose;
        public UnityEvent<bool> onMenuToggle;

        /// <summary>
        /// To check if it still needs to use its own closing method or that another script has called this one Close() / Open() method so it wont need to do its own closingmethod
        /// </summary>
        private bool doClosingMethod = true;

        public virtual void Awake()
        {
            if(doClosingMethod && initializationMode == InitializationMode.onAwake) Close();
        }

        public virtual void Start()
        {
            if(doClosingMethod && initializationMode == InitializationMode.onStart) Close();
        }

        /// <summary>
        /// Open the menu
        /// </summary>
        public virtual void Open()
        {
            doClosingMethod = false;
            Toggle(true);
            onMenuOpen?.Invoke();
        }

        /// <summary>
        /// Close the menu
        /// </summary>
        public virtual void Close()
        {
            doClosingMethod = false;
            Toggle(false);
            onMenuClose?.Invoke();
        }

        /// <summary>
        /// Toggle the menu
        /// </summary>
        /// <param name="setActive">Should the menu be set active or not?</param>
        protected virtual void Toggle(bool setActive)
        {
            if(toggleChildren)
            {
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(setActive);
                }
            }

            onMenuToggle?.Invoke(setActive);
        }

        /// <summary>
        /// The way this menu closes on application start
        /// </summary>
        [System.Serializable]
        public enum InitializationMode
        {
            dontClose,
            onAwake,
            onStart
        }
    }
}
