using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// A menu for UI navigator
    /// </summary>
    [System.Serializable]
    [AddComponentMenu("SLIDDES/UI/Navigator Menu")]
    public class NavigatorMenu : MonoBehaviour
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

        private void Awake()
        {
            if(doClosingMethod && initializationMode == InitializationMode.onAwake) Close();
        }

        private void Start()
        {
            if(doClosingMethod && initializationMode == InitializationMode.onStart) Close();
        }

        /// <summary>
        /// Open the menu
        /// </summary>
        public void Open()
        {
            if(NavigatorManager.CurrentMenu != this)
            {
                NavigatorManager.Open(this);
                return;
            }

            doClosingMethod = false;
            Toggle(true);
            onMenuOpen?.Invoke();
        }

        /// <summary>
        /// Close the menu
        /// </summary>
        public void Close()
        {
            if(NavigatorManager.CurrentMenu == this)
            {
                NavigatorManager.Instance.currentMenu = null;
                NavigatorManager.Close();
                return;
            }

            doClosingMethod = false;
            Toggle(false);
            onMenuClose?.Invoke();
        }

        /// <summary>
        /// Toggle the menu
        /// </summary>
        /// <param name="setActive">Should the menu be set active or not?</param>
        private void Toggle(bool setActive)
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