using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// For creating a gameobject in the hierarchy window
    /// </summary>
    public static class HierarchyCreate
    {
        [MenuItem("SLIDDES/UI/UI Navigator")]
        [MenuItem("GameObject/UI/SLIDDES/UI Navigator", priority = 10)]
        private static void CreateUINavigator()
        {
            GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("[UI Navigator]"));
            PrefabUtility.UnpackPrefabInstance(a, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        [MenuItem("SLIDDES/UI/UI Navigator Menu")]
        [MenuItem("GameObject/UI/SLIDDES/UI Navigator Menu", priority = 10)]
        private static void CreateUINavigatorMenu()
        {
            Transform parentCanvas = GameObject.FindObjectOfType<Canvas>().transform;
            GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("[Menu] Navigator Menu"), parentCanvas);
            PrefabUtility.UnpackPrefabInstance(a, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
    }
}
