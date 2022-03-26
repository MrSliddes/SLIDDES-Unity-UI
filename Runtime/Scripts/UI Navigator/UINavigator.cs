using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// For navigating through UI
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/UI Navigator")]
    public class UINavigator : MonoBehaviour
    {
        public UINavigatorMenu CurrentMenu { get { return currentMenu; } }

        [Tooltip("The menu to open on game start.")]
        [SerializeField] private UINavigatorMenu openingsMenu;

        /// <summary>
        /// The current opend menu
        /// </summary>
        private UINavigatorMenu currentMenu;

        // Start is called before the first frame update
        void Start()
        {
            if(openingsMenu != null) Open(openingsMenu);
        }

        /// <summary>
        /// Open a menu
        /// </summary>
        /// <param name="newMenu">The new menu to open. If null it will only close the current menu</param>
        public void Open(UINavigatorMenu newMenu)
        {
            // First close previous menu
            currentMenu?.Close();

            // Set new menu
            currentMenu = newMenu;

            // If new menu is null close current
            if(newMenu == null) return;

            // Open new menu
            currentMenu.Open();
        }

        /// <summary>
        /// Closes the current menu
        /// </summary>
        public void Close()
        {
            Open(null);
        }
    }
}