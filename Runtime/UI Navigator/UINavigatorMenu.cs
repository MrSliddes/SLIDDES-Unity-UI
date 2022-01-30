using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI.Navigator
{
    [System.Serializable]
    public class UINavigatorMenu : MonoBehaviour
    {
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