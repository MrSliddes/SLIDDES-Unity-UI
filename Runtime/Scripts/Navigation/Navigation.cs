using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    /// <summary>
    /// For navigating UI
    /// </summary>
    [System.Serializable]
    [AddComponentMenu("SLIDDES/UI/Navigation/Navigation")]
    public class Navigation : UIBehaviour, 
        IMoveHandler,
        IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler,
        ISubmitHandler
    {
        /// <summary>
        /// Is this button interactable?
        /// </summary>
        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
                onInteractable?.Invoke(interactable);
            }
        }


        [Tooltip("Is this button interactable?")]
        [SerializeField] private bool interactable = true;

        [SerializeField] private Navigation navigateUp;
        [SerializeField] private Navigation navigateDown;
        [SerializeField] private Navigation navigateLeft;
        [SerializeField] private Navigation navigateRight;

        [SerializeField] private Selectable selectOnUp;
        [SerializeField] private Selectable selectOnDown;
        [SerializeField] private Selectable selectOnLeft;
        [SerializeField] private Selectable selectOnRight;

        public UnityEvent<bool> onInteractable;

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            editorInteractable = interactable;
#endif
        }

#if UNITY_EDITOR
        private bool editorInteractable;

        protected override void OnValidate()
        {
            if(interactable != editorInteractable)
            {
                Interactable = interactable;
                editorInteractable = interactable;
            }
        }
#endif

        /// <summary>
        /// Selects this navigation
        /// </summary>
        public virtual void Select()
        {
            if(EventSystem.current == null || EventSystem.current.alreadySelecting) return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public virtual void Deselect()
        {
            if(EventSystem.current == null || EventSystem.current.alreadySelecting) return;

            EventSystem.current.SetSelectedGameObject(null);
        }

        protected virtual Navigation GetNavigation(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.Left => navigateLeft,
                MoveDirection.Up => navigateUp,
                MoveDirection.Right => navigateRight,
                MoveDirection.Down => navigateDown,
                _ => null,
            };
        }

        protected virtual void Navigate(AxisEventData eventData)
        {
            if(eventData == null) return;

            switch(eventData.moveDir)
            {
                case MoveDirection.Left:
                    if(navigateLeft != null && navigateLeft.IsActive()) navigateLeft.Select();
                    else if(selectOnLeft != null && selectOnLeft.IsActive()) selectOnLeft.Select();
                    break;
                case MoveDirection.Up:
                    if(navigateUp != null && navigateUp.IsActive()) navigateUp.Select();
                    else if(selectOnUp != null && selectOnUp.IsActive()) selectOnUp.Select();
                    break;
                case MoveDirection.Right:
                    if(navigateRight != null && navigateRight.IsActive()) navigateRight.Select();
                    else if(selectOnRight != null && selectOnRight.IsActive()) selectOnRight.Select();
                    break;
                case MoveDirection.Down:
                    if(navigateDown != null && navigateDown.IsActive()) navigateDown.Select();
                    else if(selectOnDown != null && selectOnDown.IsActive()) selectOnDown.Select();
                    break;
                default:
                    break;
            }
        }    

        protected virtual void OnMove(AxisEventData eventData)
        {
            Navigate(eventData);
        }

        protected virtual void OnPointerClick(PointerEventData eventData)
        {
            
        }

        protected virtual void OnPointerDown(PointerEventData eventData)
        {
            
        }

        protected virtual void OnPointerUp(PointerEventData eventData)
        {
            
        }

        protected virtual void OnPointerEnter(PointerEventData eventData)
        {
            Select();
        }

        protected virtual void OnPointerExit(PointerEventData eventData)
        {
            
        }

        protected virtual void OnSelect(BaseEventData eventData)
        {
            
        }

        protected virtual void OnDeselect(BaseEventData eventData)
        {
            
        }

        protected virtual void OnSubmit(BaseEventData eventData)
        {
            
        }

        #region Interfaces

        void IMoveHandler.OnMove(AxisEventData eventData)
        {
            if(!Interactable) return;
            OnMove(eventData);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if(!Interactable) return;
            OnPointerClick(eventData);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if(!Interactable) return;
            OnPointerDown(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if(!Interactable) return;
            OnPointerUp(eventData);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if(!Interactable) return;
            OnPointerEnter(eventData);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if(!Interactable) return;
            OnPointerExit(eventData);
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            if(!Interactable) return;
            OnSelect(eventData);
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            if(!Interactable) return;
            OnDeselect(eventData);
        }

        void ISubmitHandler.OnSubmit(BaseEventData eventData)
        {
            if(!Interactable) return;
            OnSubmit(eventData);
        }

        #endregion

        public enum Mode
        {
            None,
            Explicit
        }
    }
}
