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
    [AddComponentMenu("SLIDDES/UI/UI Navigator Menu")]
    public class UINavigatorMenu : MonoBehaviour
    {
        [Tooltip("The way this menu closes on application start")]
        [SerializeField] public BeginClosingMethod beginClosingMethod;
        [Tooltip("Include the gameobject where the UINavigatorMenu is attached too to be turned on/off")]
        [SerializeField] private bool includeClosingSelf;

        [Header("Callbacks")]
        [Tooltip("The elements of the menu to toggle")]
        public GameObject[] elementsToToggle;
        [Tooltip("Extra menus to open when this menu opens (and close when this menu closes)")]
        public UINavigatorMenu[] menusToOpen;
        [Tooltip("Extra menus to close when this menu opens (and open when this menu closes")]
        public UINavigatorMenu[] menusToClose;
        [Space()]
        public UnityEvent onMenuOpen;
        public UnityEvent onMenuClose;

        /// <summary>
        /// To check if it still needs to use its own closing method or that another script has called this one Close() / Open() method so it wont need to do its own closingmethod
        /// </summary>
        private bool doClosingMethod = true;

        private void Awake()
        {
            if(doClosingMethod && beginClosingMethod == BeginClosingMethod.onAwake) Close();
        }

        private void Start()
        {
            if(doClosingMethod && beginClosingMethod == BeginClosingMethod.onStart) Close();
        }

        /// <summary>
        /// Open the menu
        /// </summary>
        public void Open()
        {
            doClosingMethod = false;

            if(includeClosingSelf) gameObject.SetActive(true);

            foreach(GameObject item in elementsToToggle)
            {
                if(item != null) item.SetActive(true);
            }

            foreach(var item in menusToOpen)
            {
                if(item != null) item.Open();
            }

            foreach(var item in menusToClose)
            {
                if(item != null) item.Close();
            }

            onMenuOpen?.Invoke();
        }

        /// <summary>
        /// Close the menu
        /// </summary>
        public void Close()
        {
            doClosingMethod = false;

            if(includeClosingSelf) gameObject.SetActive(false);

            foreach(GameObject item in elementsToToggle)
            {
                if(item != null) item.SetActive(false);
            }

            foreach(var item in menusToOpen)
            {
                if(item != null) item.Close();
            }

            foreach(var item in menusToClose)
            {
                if(item != null) item.Open();
            }

            onMenuClose?.Invoke();
        }

        /// <summary>
        /// Only Closes all own menu elements
        /// </summary>
        public void CloseSelf(bool invokeOnMenuClose = false)
        {
            if(includeClosingSelf) gameObject.SetActive(false);

            foreach(GameObject item in elementsToToggle)
            {
                if(item != null) item.SetActive(false);
            }

            if(invokeOnMenuClose) onMenuClose?.Invoke();
        }

        /// <summary>
        /// The way this menu closes on application start
        /// </summary>
        [System.Serializable]
        public enum BeginClosingMethod
        {
            dontClose,
            onAwake,
            onStart
        }
    }
}