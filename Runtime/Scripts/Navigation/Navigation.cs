using System;
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
        IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler,
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
                return interactable && isActiveAndEnabled;
            }
            set
            {
                interactable = value;
                onInteractable?.Invoke(interactable);
            }
        }
        /// <summary>
        /// Is this navigation selected from a group?
        /// </summary>
        public bool IsSelectedFromGroup { get; private set; } = false;
        public bool NavigateUpTriggerSubmit
        {
            get
            {
                return navigateUpTriggerSubmit;
            }
        }
        public bool NavigateDownTriggerSubmit
        {
            get
            {
                return navigateDownTriggerSubmit;
            }
        }
        public bool NavigateLeftTriggerSubmit
        {
            get
            {
                return navigateLeftTriggerSubmit;
            }
        }
        public bool NavigateRightTriggerSubmit
        {
            get
            {
                return navigateRightTriggerSubmit;
            }
        }

        public Mode NavigationMode
        {
            get
            {
                return mode;
            }
        }

        public UnityAction OnActionEnter;
        public UnityAction OnActionSelect;

        public Navigation NavigateUp
        {
            get
            {
                return navigateUp;
            }
            set
            {
                navigateUp = value;
            }
        }
        public Navigation NavigateDown
        {
            get
            {
                return navigateDown;
            }
            set
            {
                navigateDown = value;
            }
        }
        public Navigation NavigateLeft
        {
            get
            {
                return navigateLeft;
            }
            set
            {
                navigateLeft = value;
            }
        }
        public Navigation NavigateRight
        {
            get
            {
                return navigateRight;
            }
            set
            {
                navigateRight = value;
            }
        }

        [Tooltip("Is this button interactable?")]
        [SerializeField] private bool interactable = true;

        [SerializeField] private Mode mode = Mode.Automatic;

        [SerializeField] private Navigation navigateUp;
        [SerializeField] private Selectable selectOnDown;
        [SerializeField] private Navigation navigateLeft;
        [SerializeField] private Navigation navigateRight;

        [SerializeField] private Selectable selectOnUp;
        [SerializeField] private Navigation navigateDown;
        [SerializeField] private Selectable selectOnLeft;
        [SerializeField] private Selectable selectOnRight;

        [Tooltip("If wanting to navigate up when this is selected, trigger submit() instead")]
        [SerializeField] private bool navigateUpTriggerSubmit;
        [Tooltip("If wanting to navigate down when this is selected, trigger submit() instead")]
        [SerializeField] private bool navigateDownTriggerSubmit;
        [Tooltip("If wanting to navigate left when this is selected, trigger submit() instead")]
        [SerializeField] private bool navigateLeftTriggerSubmit;
        [Tooltip("If wanting to navigate right when this is selected, trigger submit() instead")]
        [SerializeField] private bool navigateRightTriggerSubmit;

        [Tooltip("Select this as the selected value from the group")]
        [SerializeField] private bool selectFirstFromGroup;
        [Tooltip("The group this navigation belongs too")]
        [SerializeField] private List<Navigation> group = new List<Navigation>();

        public UnityEvent<bool> onInteractable;

        protected static Navigation[] navigations = new Navigation[10];
        protected static int navigationsCount = 0;
        private static Navigation[] AllActiveNavigation
        {
            get
            {
                Navigation[] temp = new Navigation[navigationsCount];
                Array.Copy(navigations, temp, navigationsCount);
                return temp;
            }
        }

        private bool onEnabledCalled;
        protected int navigationsIndex;
        protected Canvas canvas;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(!onEnabledCalled)
            {
                AddToNavigations();
                if(canvas == null) canvas = GetComponentInParent<Canvas>();
                onEnabledCalled = true;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(onEnabledCalled)
            {
                RemoveFromNavigations();
                onEnabledCalled = false;
            }
        }

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            editorInteractable = interactable;
#endif
            if(selectFirstFromGroup)
            {
                SelectFromGroup();
            }
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

        public virtual void AddToNavigations()
        {
            // Check if we need to expand the array
            if(navigationsCount == navigations.Length)
            {
                Navigation[] temp = new Navigation[navigations.Length * 2];
                Array.Copy(navigations, temp, navigations.Length);
                navigations = temp;
            }

            // Add
            navigationsIndex = navigationsCount;
            navigations[navigationsIndex] = this;
            navigationsCount++;
        }

        public virtual void RemoveFromNavigations()
        {
            navigationsCount--;
            // Update the last elements index to be this index
            navigations[navigationsCount].navigationsIndex = navigationsIndex;
            // Swap the last element and this element
            navigations[navigationsIndex] = navigations[navigationsCount];
            // Null out last element
            navigations[navigationsCount] = null;
        }

        /// <summary>
        /// Selects this navigation
        /// </summary>
        public virtual void Select()
        {
            if(EventSystem.current == null || EventSystem.current.alreadySelecting) return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <summary>
        /// Deselect this. Only works if EventSystem has this gameobject as currentSelectedGameObject.
        /// </summary>
        /// <remarks>For just deselecting this particluar gameobject, see <see cref="OnDeselect(BaseEventData)"/></remarks>
        public virtual void Deselect()
        {
            if(EventSystem.current == null || EventSystem.current.alreadySelecting) return;

            if(EventSystem.current.gameObject == gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public virtual void SelectFromGroup()
        {

        }

        public virtual void DeselectFromGroup()
        {

        }

        public virtual void AutoAssignGroup()
        {
            Transform parent = transform.parent;
            if(parent == null) return;

            group.Clear();
            foreach(Transform child in parent)
            {
                Navigation navigation = child.GetComponent<Navigation>();
                if(navigation == null) continue;
                group.Add(navigation);
            }
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

        protected virtual void Navigate(AxisEventData eventData, Navigation navigation)
        {
            if(eventData == null) return;

            if(navigation != null && navigation.IsActive())
            {
                eventData.selectedObject = navigation.gameObject;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <remarks>Copied from UnityEngine.UI.Selectable.FindSelectable()</remarks>
        /// <returns></returns>
        public Navigation FindNavigation(Vector3 dir)
        {
            // TODO THIS UNITY ALGORITIM IS GARBAGE AND NEEDS TO BE REPLACED!!!

            dir = dir.normalized;
            Vector3 localDir = Quaternion.Inverse(transform.rotation) * dir;
            Vector3 pos = transform.TransformPoint(GetPointOnRectEdge(transform as RectTransform, localDir));
            //Vector3 pos = transform.position as RectTransform;
            float maxScore = Mathf.NegativeInfinity;
            //float maxFurthestScore = Mathf.NegativeInfinity;
            float score = 0;

            //bool wantsWrapAround = navigation.wrapAround && (m_Navigation.mode == Navigation.Mode.Vertical || m_Navigation.mode == Navigation.Mode.Horizontal);

            Navigation bestPick = null;
            //Navigation bestFurthestPick = null;

            for(int i = 0; i < navigationsCount; ++i)
            {
                Navigation nav = navigations[i];

                // Dont navigate to another canvas
                if(nav.canvas != canvas) continue;

                if(nav == this) continue;
                if(!nav.Interactable || nav.mode == Mode.None) continue;

#if UNITY_EDITOR
                // Apart from runtime use, FindSelectable is used by custom editors to
                // draw arrows between different selectables. For scene view cameras,
                // only selectables in the same stage should be considered.
                //if(Camera.current != null && !UnityEditor.SceneManagement.StageUtility.IsGameObjectRenderedByCamera(sel.gameObject, Camera.current))
                //    continue;
#endif

                RectTransform navRect = nav.transform as RectTransform;
                Vector3 navCenter = navRect != null ? (Vector3)navRect.rect.center : Vector3.zero;
                Vector3 myVector = nav.transform.TransformPoint(navCenter) - pos;

                // Value that is the distance out along the direction.
                float dot = Vector3.Dot(dir, myVector);

                // If element is in wrong direction and we have wrapAround enabled check and cache it if furthest away.
                //if(wantsWrapAround && dot < 0)
                //{
                //    score = -dot * myVector.sqrMagnitude;

                //    if(score > maxFurthestScore)
                //    {
                //        maxFurthestScore = score;
                //        bestFurthestPick = sel;
                //    }

                //    continue;
                //}

                // Skip elements that are in the wrong direction or which have zero distance.
                // This also ensures that the scoring formula below will not have a division by zero error.
                if(dot <= 0) continue;

                // This scoring function has two priorities:
                // - Score higher for positions that are closer.
                // - Score higher for positions that are located in the right direction.
                // This scoring function combines both of these criteria.
                // It can be seen as this:
                //   Dot (dir, myVector.normalized) / myVector.magnitude
                // The first part equals 1 if the direction of myVector is the same as dir, and 0 if it's orthogonal.
                // The second part scores lower the greater the distance is by dividing by the distance.
                // The formula below is equivalent but more optimized.
                //
                // If a given score is chosen, the positions that evaluate to that score will form a circle
                // that touches pos and whose center is located along dir. A way to visualize the resulting functionality is this:
                // From the position pos, blow up a circular balloon so it grows in the direction of dir.
                // The first Selectable whose center the circular balloon touches is the one that's chosen.
                score = dot / myVector.sqrMagnitude;

                if(score > maxScore)
                {
                    maxScore = score;
                    bestPick = nav;
                }
            }

            //if(wantsWrapAround && null == bestPick) return bestFurthestPick;

            return bestPick;
        }

        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if(rect == null) return Vector3.zero;

            if(dir != Vector2.zero) dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }

        protected virtual Navigation GetNavigationUp()
        {
            if(navigateUpTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(mode == Mode.Explicit)
            {
                return navigateUp;
            }
            if(navigateUp != null)
            {
                return navigateUp;
            }
            if(selectOnUp != null)
            {
                selectOnUp.Select();
                return null;
            }
            if((mode & Mode.Vertical) != 0)
            {
                return FindNavigation(transform.rotation * Vector3.up);
            }
            return null;
        }

        protected virtual Navigation GetNavigationDown()
        {
            if(navigateDownTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(mode == Mode.Explicit)
            {
                return navigateDown;
            }
            if(navigateDown != null)
            {
                return navigateDown;
            }
            if(selectOnDown != null)
            {
                selectOnDown.Select();
                return null;
            }
            if((mode & Mode.Vertical) != 0)
            {
                return FindNavigation(transform.rotation * Vector3.down);
            }
            return null;
        }

        protected virtual Navigation GetNavigationLeft()
        {
            if(navigateLeftTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(mode == Mode.Explicit)
            {
                return navigateLeft;
            }
            if(navigateLeft != null)
            {
                return navigateLeft;
            }
            if(selectOnLeft != null)
            {
                selectOnLeft.Select();
                return null;
            }
            if((mode & Mode.Horizontal) != 0)
            {
                return FindNavigation(transform.rotation * Vector3.left);
            }
            return null;
        }

        protected virtual Navigation GetNavigationRight()
        {
            if(navigateRightTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(mode == Mode.Explicit)
            {
                return navigateRight;
            }
            if(navigateRight != null)
            {
                return navigateRight;
            }
            if(selectOnRight != null)
            {
                selectOnRight.Select();
                return null;
            }
            if((mode & Mode.Horizontal) != 0)
            {
                return FindNavigation(transform.rotation * Vector3.right);
            }
            return null;
        }

        protected virtual void OnMove(AxisEventData eventData)
        {
            switch(eventData.moveDir)
            {
                case MoveDirection.Up:
                    Navigate(eventData, GetNavigationUp());
                    break;
                case MoveDirection.Down:
                    Navigate(eventData, GetNavigationDown());
                    break;
                case MoveDirection.Left:
                    Navigate(eventData, GetNavigationLeft());
                    break;
                case MoveDirection.Right:
                    Navigate(eventData, GetNavigationRight());
                    break;
                default:
                    break;
            }
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
            OnActionEnter?.Invoke();
        }

        protected virtual void OnPointerMove(PointerEventData eventData)
        {

        }

        protected virtual void OnPointerExit(PointerEventData eventData)
        {

        }

        protected virtual void OnSelect(BaseEventData eventData)
        {
            if(group.Count > 0)
            {
                foreach(var item in group)
                {
                    item.IsSelectedFromGroup = false;
                    item.DeselectFromGroup();
                }
                IsSelectedFromGroup = true;
            }
            OnActionSelect?.Invoke();
        }

        public virtual void OnDeselect(BaseEventData eventData)
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

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            if(!Interactable) return;
            OnPointerMove(eventData);
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

        [Flags]
        public enum Mode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Automatic = 3,
            Explicit = 4
        }
    }
}
