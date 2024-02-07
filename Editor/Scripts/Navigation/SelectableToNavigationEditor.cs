using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SLIDDES.UI.Editor
{
    [CustomEditor(typeof(SelectableToNavigation))]
    public class SelectableToNavigationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Dont draw anything
        }
    }
}
