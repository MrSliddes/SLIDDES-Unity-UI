using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    /// <summary>
    /// For refreshing content in a Content size fitter component
    /// </summary>
    /// <credit>https://forum.unity.com/threads/content-size-fitter-refresh-problem.498536/</credit>
    public static class ContentSizeFitterRefresh
    {
        public static void Refresh(Transform transform)
        {
            if(transform == null || !transform.gameObject.activeSelf) return;

            foreach(RectTransform item in transform)
            {
                Refresh(item);
            }

            LayoutGroup layoutGroup = transform.GetComponent<LayoutGroup>();
            if(layoutGroup != null)
            {
                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }

            ContentSizeFitter contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
            if(contentSizeFitter != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            }
        }
    }
}
