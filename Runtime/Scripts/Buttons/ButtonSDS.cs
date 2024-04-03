using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Buttons/ButtonSDS")]
    public class ButtonSDS : Navigation
    {
        public bool AwaitingPressedTime { get; private set; } = false;

        public Transition.Type TransitionType
        {
            get
            {
                return transitionType;
            }
            set
            {
                transitionType = value;
            }
        }
        public TransitionProcedural TransitionProcedural
        {
            get
            {
                return transitionProcedural;
            }
            set
            {
                transitionProcedural = value;
            }
        }
        public TransitionColor TransitionColor
        {
            get
            {
                return transitionColor;
            }
            set
            {
                transitionColor = value;
            }
        }
        public TransitionSprite TransitionSprite
        {
            get
            {
                return transitionSprite;
            }
            set
            {
                transitionSprite = value;
            }
        }
        public TransitionAnimation TransitionAnimation
        {
            get
            {
                return transitionAnimation;
            }
            set
            {
                transitionAnimation = value;
            }
        }
        public TransitionText TransitionText
        {
            get
            {
                return transitionText;
            }
            set
            {
                transitionText = value;
            }
        }

        public TransitionRendering TransitionRendering
        {
            get
            {
                return transitionRendering;
            }
            set
            {
                transitionRendering = value;
            }
        }

        [SerializeField] private float timeBetweenPresses = 0;
        [SerializeField] private Transition.Type transitionType = Transition.Type.procedural;
        [SerializeField] private TransitionProcedural transitionProcedural;
        [SerializeField] private TransitionColor transitionColor;
        [SerializeField] private TransitionSprite transitionSprite;
        [SerializeField] private TransitionAnimation transitionAnimation;
        [SerializeField] private TransitionText transitionText;
        [SerializeField] private TransitionRendering transitionRendering;
        [SerializeField] private TransitionAudio transitionAudio;

        [SerializeField] private InputAction inputAction;
        [SerializeField] private InputActionPhase inputPhaseCallbackIARM = InputActionPhase.Canceled;
        [SerializeField] private InputActionReferenceMultiplayer inputActionReferenceMultiplayer;

        public UnityEvent<AxisEventData> onMove;
        public UnityEvent<PointerEventData> onClick;
        public UnityEvent<PointerEventData> onPointerDown;
        public UnityEvent<PointerEventData> onPointerUp;
        public UnityEvent<PointerEventData> onPointerEnter;
        public UnityEvent<PointerEventData> onPointerExit;
        public UnityEvent<BaseEventData> onSelect;
        public UnityEvent<BaseEventData> onDeselect;
        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;

        [SerializeField] private float eventDelayOnClick;
        [SerializeField] private float eventDelayOnPointerDown;
        [SerializeField] private float eventDelayOnPointerUp;
        [SerializeField] private float eventDelayOnPointerEnter;
        [SerializeField] private float eventDelayOnPointerExit;
        [SerializeField] private float eventDelayOnSelect;
        [SerializeField] private float eventDelayOnDeselect;
        [SerializeField] private float eventDelayOnHoverEnter;
        [SerializeField] private float eventDelayOnHoverExit;

        private bool allowedUpAfterPress = true;

        private bool notifyEvents = true;
        /// <summary>
        /// Check if the pointer has enterd this button. Default value is set to true to prevent a doubble trigger of Enter() by OnPointerEnter & OnSelect
        /// </summary>
        private bool pointerHasEnterd = true;
        private System.Action<InputAction.CallbackContext> actionExternalSubmit;
        private Coroutine coroutineSubmit;
        private Coroutine coroutineUnityEventDelayed;
        private Coroutine coroutineAwaitTimeBetweenPress;
        private ScrollRect scrollRect;

        private List<Transition> transitions = new List<Transition>();


        protected override void Awake()
        {
            base.Awake();

            actionExternalSubmit = x =>
            {
                if(Interactable)
                {
                    OnSubmit(null);
                }
            };

            inputActionReferenceMultiplayer.Callback = x =>
            {
                switch(inputPhaseCallbackIARM)
                {
                    case InputActionPhase.Disabled:
                        if(Interactable && x.phase == InputActionPhase.Disabled)
                        {
                            OnSubmit(null);
                        }
                        break;
                    case InputActionPhase.Waiting:
                        if(Interactable && x.phase == InputActionPhase.Waiting)
                        {
                            OnSubmit(null);
                        }
                        break;
                    case InputActionPhase.Started:
                        if(Interactable && x.phase == InputActionPhase.Started)
                        {
                            OnSubmit(null);
                        }
                        break;
                    case InputActionPhase.Performed:
                        if(Interactable && x.phase == InputActionPhase.Performed)
                        {
                            OnSubmit(null);
                        }
                        break;
                    case InputActionPhase.Canceled:
                        if(Interactable && x.phase == InputActionPhase.Canceled)
                        {
                            OnSubmit(null);
                        }
                        break;
                    default:
                        break;
                }
            };

            onInteractable.AddListener(x =>
            {
                if(x && gameObject.activeSelf)
                {
                    if(inputAction != null)
                    {
                        inputAction.canceled += actionExternalSubmit;
                    }
                    inputActionReferenceMultiplayer.Enable();
                }
                else
                {
                    if(inputAction != null)
                    {
                        inputAction.canceled -= actionExternalSubmit;
                    }
                    inputActionReferenceMultiplayer.Disable();
                }
            });

            // Initialize
            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                transitions.Add(transitionProcedural);
            }
            if(transitionType.HasFlag(Transition.Type.color))
            {
                transitions.Add(transitionColor);
            }
            if(transitionType.HasFlag(Transition.Type.animation))
            {
                transitions.Add(transitionAnimation);
            }
            if(transitionType.HasFlag(Transition.Type.sprite))
            {
                transitions.Add(transitionSprite);
            }
            if(transitionType.HasFlag(Transition.Type.text))
            {
                transitions.Add(transitionText);
            }
            if(transitionType.HasFlag(Transition.Type.rendering))
            {
                transitions.Add(transitionRendering);
            }
            if(transitionType.HasFlag(Transition.Type.audio))
            {
                transitions.Add(transitionAudio);
            }

            foreach(var transition in transitions)
            {
                transition.Initialize(this);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if(Interactable)
            {
                if(inputAction != null)
                {
                    inputAction.canceled += actionExternalSubmit;
                }
                inputActionReferenceMultiplayer.Enable();
            }

            foreach(var transition in transitions)
            {
                transition.OnEnable();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if(coroutineAwaitTimeBetweenPress != null)
            {
                StopCoroutine(coroutineAwaitTimeBetweenPress);
                AwaitingPressedTime = false;
                allowedUpAfterPress = true;
            }

            if(inputAction != null)
            {
                inputAction.canceled -= actionExternalSubmit;
            }
            inputActionReferenceMultiplayer.Disable();

            foreach(var transition in transitions)
            {
                transition.OnDisable();
            }
        }

        public void Press()
        {
            if(AwaitingPressedTime) return;

            notifyEvents = true;
            OnSubmit(null);
        }

        /// <summary>
        /// Press the button and only show visual feedback but no events will be triggerd
        /// </summary>
        public void PressNoNotify()
        {
            if(AwaitingPressedTime) return;

            notifyEvents = false;
            OnSubmit(null);
        }

        public void SetNavigationFromSiblings()
        {
            int siblingIndex = transform.GetSiblingIndex();
            int siblingAbove = siblingIndex - 1;
            int siblingBelow = siblingIndex + 1;

            // Get sibling above
            if(siblingAbove >= 0)
            {
                Navigation navigation = transform.parent.GetChild(siblingAbove).GetComponent<Navigation>();
                if(navigation != null)
                {
                    NavigateUp = navigation;
                }
            }

            // Get sibling below
            if(siblingBelow < transform.parent.childCount)
            {
                Navigation navigation = transform.parent.GetChild(siblingBelow).GetComponent<Navigation>();
                if(navigation != null)
                {
                    NavigateDown = navigation;
                }
            }
        }

        private void OnPointerDown()
        {
            foreach(var transition in transitions)
            {
                transition.PointerDown();
            }

            AwaitTimeBetweenPress();
        }

        private void OnPointerUp()
        {
            foreach(var transition in transitions)
            {
                transition.PointerUp();
            }
        }

        public void Enter(BaseEventData eventData)
        {
            foreach(Transition transition in transitions)
            {
                transition.Enter();
            }
        }

        public void Exit(BaseEventData eventData)
        {
            foreach(Transition transition in transitions)
            {
                transition.Exit();
            }
        }

        protected override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            onMove?.Invoke(eventData);
        }

        protected override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if(AwaitingPressedTime) return;

            UnityEventDelayed(() => onClick?.Invoke(eventData), eventDelayOnClick);
        }

        protected override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if(AwaitingPressedTime) return;

            OnPointerDown();
            UnityEventDelayed(() => onPointerDown?.Invoke(eventData), eventDelayOnPointerDown);
        }

        protected override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if(AwaitingPressedTime && !allowedUpAfterPress) return;

            allowedUpAfterPress = false;

            if(eventData != null)
            {
                // Check when releasing the pointer the pointer is still hoverd on this button
                if(!eventData.hovered.Contains(gameObject)) return;
                // Set this gameobject as the selected gameobject (silently)
                eventData.selectedObject = gameObject;
            }

            OnPointerUp();
            UnityEventDelayed(() => onPointerUp?.Invoke(eventData), eventDelayOnPointerUp);

            // Reset notifyEvents
            notifyEvents = true;
        }

        protected override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            // If the pointer has already enterd via OnSelect dont trigger Enter() again
            if(!pointerHasEnterd)
            {
                pointerHasEnterd = true;
                Enter(eventData);
            }
            UnityEventDelayed(() => onPointerEnter?.Invoke(eventData), eventDelayOnPointerEnter);
        }

        protected override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            pointerHasEnterd = false;

            if(!IsSelectedFromGroup) Exit(eventData);
            UnityEventDelayed(() => onPointerExit?.Invoke(eventData), eventDelayOnPointerExit);
        }

        protected override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            pointerHasEnterd = true;

            Enter(eventData);
            UnityEventDelayed(() => onSelect?.Invoke(eventData), eventDelayOnSelect);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            // Reset notifyEvents
            notifyEvents = true;
            // If onSubmit is busy, stop it
            if(coroutineSubmit != null) StopCoroutine(coroutineSubmit);

            if(!IsSelectedFromGroup) Exit(eventData);
            UnityEventDelayed(() => onDeselect?.Invoke(eventData), eventDelayOnDeselect);
        }

        protected override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            if(AwaitingPressedTime) return;

            if(coroutineSubmit != null) StopCoroutine(coroutineSubmit);
            coroutineSubmit = StartCoroutine(SubmitAsync(eventData));
        }

        public override void SelectFromGroup()
        {
            base.SelectFromGroup();
            Enter(null);
        }

        public override void DeselectFromGroup()
        {
            base.DeselectFromGroup();
            Exit(null);
        }

        private void AwaitTimeBetweenPress()
        {
            if(timeBetweenPresses <= 0) return;

            allowedUpAfterPress = true;
            AwaitingPressedTime = true;
            if(coroutineAwaitTimeBetweenPress != null) StopCoroutine(coroutineAwaitTimeBetweenPress);
            coroutineAwaitTimeBetweenPress = StartCoroutine(AwaitTimeBetweenPressAsync());
        }

        private IEnumerator AwaitTimeBetweenPressAsync()
        {
            yield return new WaitForSecondsRealtime(timeBetweenPresses);
            AwaitingPressedTime = false;
            allowedUpAfterPress = true;
        }

        private void UnityEventDelayed(UnityAction action, float delay, bool singular = true)
        {
            if(!notifyEvents) return;

            if(delay <= 0)
            {
                action.Invoke();
            }
            else
            {
                if(singular)
                {
                    if(coroutineUnityEventDelayed != null) StopCoroutine(coroutineUnityEventDelayed);
                    coroutineUnityEventDelayed = StartCoroutine(UnityEventDelayedAsync(action, delay));
                }
                else
                {
                    StartCoroutine(UnityEventDelayedAsync(action, delay));
                }
            }
        }

        private IEnumerator SubmitAsync(BaseEventData eventData)
        {
            OnPointerDown(null);

            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                yield return new WaitForSecondsRealtime(transitionProcedural.TimePointerDown);
            }
            else if(transitionType.HasFlag(Transition.Type.color))
            {
                yield return new WaitForSecondsRealtime(transitionColor.FadeDuration);
            }
            else if(transitionType.HasFlag(Transition.Type.animation))
            {
                yield return new WaitForSecondsRealtime(transitionAnimation.NormalizedTransitionDuration);
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }

            OnPointerUp(null);

            yield break;
        }
        
        private IEnumerator UnityEventDelayedAsync(UnityAction action, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            action.Invoke();
        }
    }
}
