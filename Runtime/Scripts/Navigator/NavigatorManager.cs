using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// For navigating through UI
    /// </summary>
    public class NavigatorManager : MonoBehaviour
    {
        public static NavigatorManager Instance 
        { 
            get
            {
                if(instance == null)
                {
                    GameObject a = new GameObject("[Navigator Manager]");
                    instance = a.AddComponent<NavigatorManager>();
                }
                return instance;
            }
        }

        public static NavigatorMenu CurrentMenu { get { return Instance.currentMenu; } }


        private static NavigatorManager instance;
        /// <summary>
        /// The current opend menu
        /// </summary>
        public NavigatorMenu currentMenu;


        /// <summary>
        /// Closes the current menu
        /// </summary>
        public static void Close()
        {
            Open(null);
        }

        /// <summary>
        /// Open a menu
        /// </summary>
        /// <param name="newMenu">The new menu to open. If null it will only close the current menu</param>
        public static void Open(NavigatorMenu newMenu)
        {
            // First close previous menu
            if(CurrentMenu != null) CurrentMenu.Close();

            // Set new menu
            Instance.currentMenu = newMenu;

            // If new menu is null close current
            if(newMenu == null) return;

            // Open new menu
            Instance.currentMenu.Open();
        }
    }
}