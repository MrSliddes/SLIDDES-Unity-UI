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
        [Tooltip("Dont close the menu on start")]
        [SerializeField] private bool dontCloseOnStart;
        [Tooltip("Include the gameobject where the UINavigatorMenu is attached too to be turned on/off")]
        [SerializeField] private bool includeSelf;
        [Tooltip("The elements of the menu to toggle")]
        public GameObject[] elements;

        [Header("Callbacks")]
        public UnityEvent onMenuOpen;
        public UnityEvent onMenuClose;

        [Header("Extra Menus")]
        [Tooltip("Extra menus to open when this menu opens (and close when this menu closes)")]
        public UINavigatorMenu[] menusToOpen;
        [Tooltip("Extra menus to close when this menu opens (and open when this menu closes")]
        public UINavigatorMenu[] menusToClose;

        private void Start()
        {
            if(!dontCloseOnStart) Close();
        }

        /// <summary>
        /// Open the menu
        /// </summary>
        public void Open()
        {
            if(includeSelf) gameObject.SetActive(true);

            foreach(GameObject item in elements)
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
            if(includeSelf) gameObject.SetActive(false);

            foreach(GameObject item in elements)
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
            if(includeSelf) gameObject.SetActive(false);

            foreach(GameObject item in elements)
            {
                if(item != null) item.SetActive(false);
            }

            if(invokeOnMenuClose) onMenuClose?.Invoke();
        }
    }
}