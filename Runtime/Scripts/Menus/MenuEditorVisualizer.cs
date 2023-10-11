using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.UI;

namespace SLIDDES.UI.MenuEditorVisualizer
{
    /// <summary>
    /// A class that organises menus to easily edit them while in editor
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Menus/Menu Editor Visualizer")]
    public class MenuEditorVisualizer : MonoBehaviour
    {
        [Tooltip("Instead of text based use designated rect transforms")]
        public bool useCustomRectTransforms = false;
        [Tooltip("Should this component debug log be logged to the console?")]
        public bool debug = false;
        [Tooltip("The menu prefix string used to detect the menus (not uppercase sensitive)")]
        public string menuPrefix = "[Menu]";

        /// <summary>
        /// This rect transform
        /// </summary>
        public RectTransform rt;
        /// <summary>
        /// The recttransforms of all menus
        /// </summary>
        public List<Menu> menus = new List<Menu>();

        private Vector2Int nextMenuIndexing = Vector2Int.zero;

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
            Vector2 size = rt.rect.size;

            // Loop through all menus
            for(int i = 0; i < menus.Count; i++)
            {
                // Get local indexing
                Vector2Int indexing = menus[i].index - menus[i].parentIndex;

                // Set position and size
                menus[i].rectTransform.SetRect(
                    (indexing.y * size.y) , (indexing.y * -size.y) ,
                    (indexing.x * size.x) , (indexing.x * -size.x) );
            }

            if(debug) Debug.Log($"[Menu Editor Visualizer] Organised {menus.Count}");

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
            }

            if(debug) Debug.Log("[Menu Editor Visualizer] Resetted Menus.");
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
                nextMenuIndexing = new Vector2Int(0, 0);

                FindMenuRecursively(rt, nextMenuIndexing);
            }

            if(debug) Debug.Log("[Menu Editor Visualizer] Setup RT.");
        }
             
        /// <summary>
        /// Find menus recursively in a rectTransform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="parentIndex"></param>
        /// <param name="childIndex"></param>
        /// <param name="y"></param>
        private void FindMenuRecursively(RectTransform rectTransform, Vector2Int parentIndex, int y = 0)
        {
            foreach(Transform child in rectTransform)
            {
                if(child.name.Contains(menuPrefix))
                {
                    Vector2Int newIndex = new Vector2Int(nextMenuIndexing.x, y);
                    menus.Add(new Menu(child.GetComponent<RectTransform>(), newIndex, parentIndex));
                    nextMenuIndexing.x++;

                    FindMenuRecursively(child.GetComponent<RectTransform>(), newIndex, y + 1);
                }
            }
        }

        [System.Serializable]
        public class Menu
        {
            [Tooltip("The rect transform of the menu")]
            public RectTransform rectTransform;

            [Tooltip("The indexing of the menu used for organizing")]
            public Vector2Int index;
            [Tooltip("The index of the parent of this menu")]
            public Vector2Int parentIndex;

            public Menu(RectTransform rectTransform, Vector2Int index, Vector2Int parentIndex)
            {
                this.rectTransform = rectTransform;
                this.index = index;
                this.parentIndex = parentIndex;
            }
        }
    }
}
