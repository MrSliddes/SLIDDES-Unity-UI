using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Events;

namespace SLIDDES.UI.Editor
{
    /// <summary>
    /// Allows for re-ordering UnityEvent events in inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    public class ReorderableUnityEventDrawer : UnityEventDrawer
    {
        protected override void SetupReorderableList(ReorderableList list)
        {
            base.SetupReorderableList(list);
            list.draggable = true;
        }
    }
}
