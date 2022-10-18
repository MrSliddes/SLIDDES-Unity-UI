using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.UI;

namespace SLIDDES.UI
{
    /// <summary>
    /// Gets the safe area where the Rect transform can render in (example phones with notches in them)
    /// </summary>
    public class ScreenSafeArea : MonoBehaviour
    {
        [Tooltip("Should the safe area be refreshed every frame?")]
        [SerializeField] private bool updateEveryFrame = true;

        /// <summary>
        /// The last calculated safe area rect
        /// </summary>
        private Rect lastSafeArea;
        /// <summary>
        /// The rect transform attached to this gameobject
        /// </summary>
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if(updateEveryFrame) Refresh();
        }

        public void Refresh()
        {
            Rect safeArea = Screen.safeArea;

            if(safeArea != lastSafeArea)
            {
                // Set new rect
                lastSafeArea = safeArea;

                Vector2 anchorMin = safeArea.position;
                Vector2 anchorMax = safeArea.position + safeArea.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
            }
        }
    }
}
