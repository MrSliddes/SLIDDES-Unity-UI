using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionRendering : Transition
    {
        public Transform Transform
        {
            get
            {
                return transform;
            }
            set
            {
                transform = value;
            }
        }

        [SerializeField] private Transform transform;
        [SerializeField] private Canvas canvas;
        [Space]
        [SerializeField] private int sortOrderNormal = -1;
        [SerializeField] private int sortOrderHover = -1;
        [Space]
        [SerializeField] private int siblingIndexHover = -1;
        [Space]
        [Tooltip("When this gets selected, find scroll rect it is in and set it centered to this. Requires transform to be set!")]
        [SerializeField] private bool onSelectSetScrollRectPosition;

        private int siblingIndex;
        private Transform parentTransform;
        private ScrollRect scrollRect;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour, objects);
            if(transform != null)
            {
                parentTransform = transform.parent;

                if(onSelectSetScrollRectPosition)
                {
                    scrollRect = transform.GetComponentInParent<ScrollRect>();
                }
            }
        }

        public override void Enter()
        {
            base.Enter();

            if(canvas != null)
            {
                canvas.sortingOrder = sortOrderHover;
            }

            if(parentTransform != null)
            {
                if(siblingIndexHover >= 0)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    transform.SetSiblingIndex(siblingIndexHover);
                }
            }        
            
            if(transform != null)
            {
                if(onSelectSetScrollRectPosition)
                {
                    // Scroll if input isnt mouse
                    if(!InputManager.CurrentInputDeviceIsMouseAndKeyboard) // TODO if input device name is keyboard, check if mouse isnt used also
                    {
                        // Make the canvas scroll to this button
                        float y = 1 - ((float)transform.GetSiblingIndex() / ((float)scrollRect.content.childCount - 1));
                        y = Mathf.Clamp01(y);
                        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, y);
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            if(canvas != null)
            {
                canvas.sortingOrder = sortOrderNormal;
            }

            if (parentTransform != null)
            {
                if(siblingIndexHover >= 0)
                {
                    // Set back
                    transform.SetSiblingIndex(siblingIndex);
                }
            }
        }
    }
}
