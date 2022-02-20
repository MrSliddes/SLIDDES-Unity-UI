using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.UI;

namespace SLIDDES.UI.MenuEditorVisualizer
{
    /// <summary>
    /// A class that organises menus to easily edit them while in editor
    /// </summary>
    public class MenuEditorVisualizer : MonoBehaviour
    {
        public bool useCustomRectTransforms = false;

        [SerializeField] private RectTransform rt;
        /// <summary>
        /// The recttransforms of all menus
        /// </summary>
        [SerializeField] private RectTransform[] menus;

        private void Awake()
        {
            SetupRectTransforms();
            if(Application.isPlaying)
            {
                ResetMenus();
            }
        }

        /// <summary>
        /// Place all menus vertically underneath eachother
        /// </summary>
        public void OrganiseMenus()
        {
#if UNITY_EDITOR
            SetupRectTransforms();
#endif
            float height = rt.rect.height;
            for(int i = 0; i < menus.Length; i++)
            {
                menus[i].SetRect(i * height, i * -height, 0, 0);
            }

            Debug.Log(string.Format("[Menu Editor Visualizer] Organised {0} Menus.", menus.Length));
        }

        /// <summary>
        /// Position all menus back to 0, 0, 0, 0
        /// </summary>
        public void ResetMenus()
        {
#if UNITY_EDITOR
            SetupRectTransforms();
#endif
            for(int i = 0; i < menus.Length; i++)
            {
                menus[i].SetRect(0, 0, 0, 0);
            }

            Debug.Log("[Menu Editor Visualizer] Resetted Menus.");
        }

        private void SetupRectTransforms()
        {
            if(useCustomRectTransforms)
            {
                if(rt == null) rt = GetComponent<RectTransform>();
            }
            else
            {
                rt = GetComponent<RectTransform>();
                menus = new RectTransform[transform.childCount];
                int i = 0;
                RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
                foreach(RectTransform child in rectTransforms)
                {
                    if(child.name.ToLower().Contains("[menu]"))
                    {
                        menus[i] = child;
                        i++;
                    }
                }
                print(i);
                System.Array.Resize(ref menus, i);
            }

            Debug.Log("[Menu Editor Visualizer] Setup RT.");
        }
    }
}
