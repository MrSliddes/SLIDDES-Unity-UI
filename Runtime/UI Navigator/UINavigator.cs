using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// For navigating through UI
    /// </summary>
    public class UINavigator : MonoBehaviour
    {
        public static UINavigator Instance;

        public static UINavigatorMenu CurrentMenu { get { return Instance.currentMenu; } }

        [Tooltip("The menu to open on game start. If left null UINavigator will pick the first menus index")]
        [SerializeField] private UINavigatorMenu openingsMenu;

        public UINavigatorMenu[] menus;

        private UINavigatorMenu currentMenu;

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            CloseAll();
            if(openingsMenu == null) Open(menus[0]); else Open(openingsMenu);
        }

        public static void CloseAll()
        {
            foreach(var item in Instance.menus)
            {
                item.CloseSelf();
            }
        }
        
        public static void Open(UINavigatorMenu menu)
        {
            // First close previous menu
            Instance.currentMenu?.Close();
            // Set new menu
            Instance.currentMenu = menu;
            // Open new menu
            Instance.currentMenu.Open();
        }

        public static void Open(int menuIndex)
        {
            // Check if index isnt out of range
            if(menuIndex < 0 || menuIndex >= Instance.menus.Length)
            {
                Debug.LogError("[UINavigator] Open(menuIndex) is out of range! Got index: " + menuIndex);
                return;
            }
            // Open Menu
            Open(Instance.menus[menuIndex]);
        }

        /// <summary>
        /// The same as the static function Open() but usable for example button.onClick()
        /// </summary>
        /// <param name="menu"></param>
        public void OpenNonStatic(UINavigatorMenu menu)
        {
            Open(menu);
        }
    }
}