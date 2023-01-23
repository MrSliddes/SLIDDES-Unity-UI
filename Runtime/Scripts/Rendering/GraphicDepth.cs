using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace SLIDDES.UI
{
    /// <summary>
    /// Sets the UI drawing depth for the camera
    /// </summary>
    /// <partial-credit>@glitchers - https://answers.unity.com/questions/878667/world-space-canvas-on-top-of-everything.html </partial-credit>
    [AddComponentMenu("SLIDDES/UI/Rendering/Graphic Depth")]
    public class GraphicDepth : MonoBehaviour, IMaterialModifier
    {
        private static int PropertyID
        {
            get
            {
                if(propertyID.HasValue == false)
                {
                    propertyID = Shader.PropertyToID(propertyKey);
                }
                return propertyID.Value;
            }
        }
        private static int? propertyID = null;

        /// <summary>
        /// Set the compare function of the graphic
        /// </summary>
        public CompareFunction CompareFunction
        {
            get { return compareFunction; }
            set
            {
                compareFunction = value;
                SetDirty();
            }
        }

        [Tooltip("LessEqual is 'normal'. Always is overlay. Never is hide.")]
        [SerializeField] private CompareFunction compareFunction = CompareFunction.Always;

        /// <summary>
        /// The property key of the shader
        /// </summary>
        private const string propertyKey = "unity_GUIZTestMode";
        private Graphic graphic;
        private Material renderMaterial;

        private void Awake()
        {
            graphic = GetComponent<Graphic>();
        }

        private void OnEnable()
        {
            SetDirty();
        }

        private void OnDisable()
        {
            SetDirty();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(graphic == null) graphic = GetComponent<Graphic>();
            SetDirty();
        }
#endif

        /// <summary>
        /// Tell unity to update the material
        /// </summary>
        private void SetDirty()
        {
            if(graphic != null) graphic.SetMaterialDirty();
        }

        Material IMaterialModifier.GetModifiedMaterial(Material baseMaterial)
        {
#if UNITY_EDITOR
            if(!Application.isPlaying) return baseMaterial;
#endif
            // Assign render material
            if(renderMaterial == null)
            {
                renderMaterial = new Material(baseMaterial)
                {
                    name = baseMaterial.name + " CameraUIDepth",
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

            renderMaterial.SetInt(PropertyID, (int)compareFunction);
            return renderMaterial;
        }
    }
}
