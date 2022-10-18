using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.UI;

namespace SLIDDES.UI.MenuEditorVisualizer
{
    /// <summary>
    /// A class that organises menus to easily edit them while in editor
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Menu Editor Visualizer")]
    public class MenuEditorVisualizer : MonoBehaviour
    {
        [Tooltip("Instead of text based use designated rect transforms")]
        public bool useCustomRectTransforms = false;
        [Tooltip("Should this component debug log be logged to the console?")]
        public bool logDebugs = false;
        [Tooltip("The menu prefix string used to detect the menus (not uppercase sensitive)")]
        public string menuPrefix = "[Menu]";
        [Tooltip("The menu prefix string used to detect the sub-menus (not uppercase sensitive)")]
        public string subMenuPrefix = "[SubMenu]";

        /// <summary>
        /// This rect transform
        /// </summary>
        public RectTransform rt;
        /// <summary>
        /// The recttransforms of all menus
        /// </summary>
        public List<Menu> menus = new List<Menu>();

        private void Awake()
        {
            GetMenus();
            ResetMenus();
        }

        /// <summary>
        /// Place all menus vertically underneath eachother
        /// </summary>
        public void OrganiseMenus()
        {
#if UNITY_EDITOR
            GetMenus();
#endif
            float height = rt.rect.height;
            for(int i = 0; i < menus.Count; i++)
            {
                // Set menu recttransform (place vertical)
                menus[i].rectTransform.SetRect(i * height, i * -height, 0, 0);
                // Set submenu recttransform (place horizontal right)
                for(int j = 0; j < menus[i].subMenus.Count; j++)
                {
                    float width = menus[i].rectTransform.rect.width;
                    menus[i].subMenus[j].SetRect(0, 0, (j + 1) * width, (j + 1) * -width);
                }
            }

            if(logDebugs) Debug.Log(string.Format("[Menu Editor Visualizer] Organised {0} Menus.", menus.Count));
        }

        /// <summary>
        /// Position all menus back to 0, 0, 0, 0
        /// </summary>
        public void ResetMenus()
        {
#if UNITY_EDITOR
            GetMenus();
#endif
            foreach(var item in menus)
            {
                item.rectTransform.SetRect(0, 0, 0, 0);
                foreach(var subMenu in item.subMenus)
                {
                    subMenu.SetRect(0, 0, 0, 0);
                }
            }

            if(logDebugs) Debug.Log("[Menu Editor Visualizer] Resetted Menus.");
        }

        /// <summary>
        /// Get all the menu/sub-menus
        /// </summary>
        private void GetMenus()
        {
            if(useCustomRectTransforms)
            {
                if(rt == null) rt = GetComponent<RectTransform>();
            }
            else
            {
                rt = GetComponent<RectTransform>();
                menus.Clear();
                foreach(Transform item in rt)
                {
                    if(item.name.ToLower().Contains(menuPrefix.ToLower()))
                    {
                        Menu menu = new Menu(item.GetComponent<RectTransform>());
                        menus.Add(menu);
                        // Get sub menus
                        foreach(Transform menuChild in item)
                        {
                            if(menuChild.name.ToLower().Contains(subMenuPrefix.ToLower()))
                            {
                                menus[menus.IndexOf(menu)].subMenus.Add(menuChild.GetComponent<RectTransform>());
                            }
                        }
                    }
                }
            }

            if(logDebugs) Debug.Log("[Menu Editor Visualizer] Setup RT.");
        }

        [System.Serializable]
        public class Menu
        {
            [Tooltip("The rect transform of the menu")]
            public RectTransform rectTransform;
            [Tooltip("The rect transforms of the menu sub-menus")]
            public List<RectTransform> subMenus = new List<RectTransform>();

            public Menu(RectTransform rectTransform)
            {
                this.rectTransform = rectTransform;
            }
        }
    }
}
