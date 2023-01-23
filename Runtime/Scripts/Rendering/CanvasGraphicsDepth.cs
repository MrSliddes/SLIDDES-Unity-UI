using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace SLIDDES.UI
{
    /// <summary>
    /// Sets all the canvas graphics depth
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Rendering/Canvas Graphics Depth")]
    public class CanvasGraphicsDepth : MonoBehaviour
    {
        [Tooltip("LessEqual is 'normal'. Always is overlay. Never is hide.")]
        public CompareFunction compareFunction = CompareFunction.Always;
        [Tooltip("Should the script also update graphics on inactive gameobjects?")]
        public bool setInactiveGraphics = true;
        [Tooltip("When this script is changed in inspector should it update?")]
        public bool updateOnValidate = false;
        [Tooltip("List of all graphics affected by script. If left blank it will automatically find graphics in its children")]
        public Graphic[] graphics;

        private void OnValidate()
        {
            if(updateOnValidate && Application.isPlaying) UpdateGraphics();
        }

        private void Start()
        {
            UpdateGraphics();
        }

        public void UpdateGraphics()
        {
            if(graphics.Length == 0) graphics = GetComponentsInChildren<Graphic>(setInactiveGraphics);

            for(int i = 0; i < graphics.Length; i++)
            {
                // Check if a graphicsCameraDepth component is added, if not add it
                GraphicDepth component = graphics[i].GetComponent<GraphicDepth>();
                if(component == null)
                {
                    component = graphics[i].gameObject.AddComponent<GraphicDepth>();
                }

                component.CompareFunction = compareFunction;
            }
        }
    }
}
