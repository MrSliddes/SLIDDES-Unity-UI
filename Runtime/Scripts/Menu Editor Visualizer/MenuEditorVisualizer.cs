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
        public string menuPrefix = "[menu]";

        [SerializeField] private RectTransform rt;
        /// <summary>
        /// The recttransforms of all menus
        /// </summary>
        [SerializeField] private List<RectTransform> menus = new List<RectTransform>();

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
            for(int i = 0; i < menus.Count; i++)
            {
                menus[i].SetRect(i * height, i * -height, 0, 0);
            }

            if(logDebugs) Debug.Log(string.Format("[Menu Editor Visualizer] Organised {0} Menus.", menus.Count));
        }

        /// <summary>
        /// Position all menus back to 0, 0, 0, 0
        /// </summary>
        public void ResetMenus()
        {
#if UNITY_EDITOR
            SetupRectTransforms();
#endif
            for(int i = 0; i < menus.Count; i++)
            {
                menus[i].SetRect(0, 0, 0, 0);
            }

            if(logDebugs) Debug.Log("[Menu Editor Visualizer] Resetted Menus.");
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
                menus.Clear();
                foreach(Transform child in rt)
                {
                    if(child.GetComponent<RectTransform>() == null) continue;
                    if(child.name.ToLower().Contains(menuPrefix))
                    {
                        menus.Add(child.GetComponent<RectTransform>());
                    }
                }
            }

            if(logDebugs) Debug.Log("[Menu Editor Visualizer] Setup RT.");
        }
    }
}
