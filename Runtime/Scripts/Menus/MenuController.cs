using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI.Menus
{
    [AddComponentMenu("SLIDDES/UI/Menus/Menu Controller")]
    public class MenuController : MonoBehaviour
    {
        [Tooltip("The current menu of the controller")]
        [SerializeField] private Menu currentMenu;
        [Tooltip("All menus of the controller")]
        [SerializeField] private List<Menu> menus = new List<Menu>();

        private bool initialized;

        private void Awake()
        {
            Initialize();
            if(currentMenu != null)
            {
                OpenMenu(currentMenu);
            }
        }
                
        /// <summary>
        /// Open an menu
        /// </summary>
        /// <param name="menu">The menu to be opend</param>
        public void OpenMenu(Menu menu)
        {
            if(!initialized) Initialize();

            if(currentMenu == menu) return;

            if(currentMenu != null) currentMenu.Close();

            currentMenu = menu;
            if(currentMenu != null) currentMenu.Open();
        }

        /// <summary>
        /// Get all menus children to this controller
        /// </summary>
        private void GetMenus()
        {
            if(menus.Count == 0) menus.AddRange(GetComponentsInChildren<Menu>());
        }

        /// <summary>
        /// Setup for the menus
        /// </summary>
        private void Setup()
        {
            for(int i = 0; i < menus.Count; i++)
            {
                if(menus[i].CloseOnStart && !menus[i].IsClosed) menus[i].Close();
            }
        }

        /// <summary>
        /// Initialize the controller
        /// </summary>
        private void Initialize()
        {
            GetMenus();
            Setup();
            initialized = true;
        }
    }
}
