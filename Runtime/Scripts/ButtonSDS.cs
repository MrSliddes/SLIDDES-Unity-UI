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
    public class ButtonSDS : Navigation
    {
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

        [SerializeField] private Transition.Type transitionType = Transition.Type.procedural;
        [SerializeField] private TransitionProcedural transitionProcedural;
        [SerializeField] private TransitionColor transitionColor;
        [SerializeField] private TransitionSprite transitionSprite;
        [SerializeField] private TransitionAnimation transitionAnimation;
        [SerializeField] private TransitionText transitionText;

        [SerializeField] private InputAction inputActionExternalSubmit;

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

        private UnityAction actionExternalSubmit;
        private Coroutine coroutineSubmit;
        private Coroutine coroutineUnityEventDelayed;

        protected override void Awake()
        {
            base.Awake();
            actionExternalSubmit = () => OnSubmit(null);

            inputActionExternalSubmit.canceled += x =>
            {
                actionExternalSubmit.Invoke();
            };

            onInteractable.AddListener(x =>
            {
                if(x && gameObject.activeSelf)
                {
                    inputActionExternalSubmit.Enable();
                }
                else
                {
                    inputActionExternalSubmit.Disable();
                }
            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if(Interactable) inputActionExternalSubmit.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            inputActionExternalSubmit.Disable();
        }

        protected override void Start()
        {
            base.Start();

            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                transitionProcedural.Initialize(this);
            }
            if(transitionType.HasFlag(Transition.Type.color))
            {
                transitionColor.Initialize(this, this);
            }
            if(transitionType.HasFlag(Transition.Type.animation))
            {
                transitionAnimation.Initialize(this);
            }
            if(transitionType.HasFlag(Transition.Type.sprite))
            {
                transitionSprite.Initialize(this, this);
            }
            if(transitionType.HasFlag(Transition.Type.text))
            {
                transitionText.Initialize(this, this);
            }
        }

        public void Press()
        {
            OnSubmit(null);
        }

        private void OnPointerDown()
        {
            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                transitionProcedural.PointerDown();
            }
            if(transitionType.HasFlag(Transition.Type.color))
            {
                transitionColor.PointerDown();
            }
            if(transitionType.HasFlag(Transition.Type.animation))
            {
                transitionAnimation.PointerDown();
            }
            if(transitionType.HasFlag(Transition.Type.sprite))
            {
                transitionSprite.PointerDown();
            }
            if(transitionType.HasFlag(Transition.Type.text))
            {
                transitionText.PointerDown();
            }
        }

        private void OnPointerUp()
        {
            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                transitionProcedural.PointerUp();
            }
            if(transitionType.HasFlag(Transition.Type.color))
            {
                transitionColor.PointerUp();
            }
            if(transitionType.HasFlag(Transition.Type.animation))
            {
                transitionAnimation.PointerUp();
            }
            if(transitionType.HasFlag(Transition.Type.sprite))
            {
                transitionSprite.PointerUp();
            }
            if(transitionType.HasFlag(Transition.Type.text))
            {
                transitionText.PointerUp();
            }
        }

        public void Enter(BaseEventData eventData)
        {
            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                transitionProcedural.Enter();
            }
            if(transitionType.HasFlag(Transition.Type.color))
            {
                transitionColor.Enter();
            }
            if(transitionType.HasFlag(Transition.Type.animation))
            {
                transitionAnimation.Enter();
            }
            if(transitionType.HasFlag(Transition.Type.sprite))
            {
                transitionSprite.Enter();
            }
            if(transitionType.HasFlag(Transition.Type.text))
            {
                transitionText.Enter();
            }
        }

        public void Exit(BaseEventData eventData)
        {
            if(transitionType.HasFlag(Transition.Type.procedural))
            {
                transitionProcedural.Exit();
            }
            if(transitionType.HasFlag(Transition.Type.color))
            {
                transitionColor.Exit();
            }
            if(transitionType.HasFlag(Transition.Type.animation))
            {
                transitionAnimation.Exit();
            }
            if(transitionType.HasFlag(Transition.Type.sprite))
            {
                transitionSprite.Exit();
            }
            if(transitionType.HasFlag(Transition.Type.text))
            {
                transitionText.Exit();
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

            UnityEventDelayed(() => onClick?.Invoke(eventData), eventDelayOnClick);
        }

        protected override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            OnPointerDown();
            UnityEventDelayed(() => onPointerDown?.Invoke(eventData), eventDelayOnPointerDown);
        }

        protected override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if(eventData != null)
            {
                // Check when releasing the pointer the pointer is still hoverd on this button
                if(!eventData.hovered.Contains(gameObject)) return;
                // Set this gameobject as the selected gameobject (silently)
                eventData.selectedObject = gameObject;
            }

            OnPointerUp();
            UnityEventDelayed(() => onPointerUp?.Invoke(eventData), eventDelayOnPointerUp);
        }

        protected override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            Enter(eventData);
            UnityEventDelayed(() => onPointerEnter?.Invoke(eventData), eventDelayOnPointerEnter);
        }

        protected override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            Exit(eventData);
            UnityEventDelayed(() => onPointerExit?.Invoke(eventData), eventDelayOnPointerExit);
        }

        protected override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            Enter(eventData);
            UnityEventDelayed(() => onSelect?.Invoke(eventData), eventDelayOnSelect);
        }

        protected override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            Exit(eventData);
            UnityEventDelayed(() => onDeselect?.Invoke(eventData), eventDelayOnDeselect);
        }

        protected override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            
            if(coroutineSubmit != null) StopCoroutine(coroutineSubmit);
            coroutineSubmit = StartCoroutine(SubmitAsync(eventData));
        }

        private void UnityEventDelayed(UnityAction action, float delay, bool singular = true)
        {
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
                yield return new WaitForSeconds(transitionProcedural.TimePointerDown);
            }
            else if(transitionType.HasFlag(Transition.Type.color))
            {
                yield return new WaitForSeconds(transitionColor.FadeDuration);
            }
            else if(transitionType.HasFlag(Transition.Type.animation))
            {
                yield return new WaitForSeconds(transitionAnimation.NormalizedTransitionDuration);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

            OnPointerUp(null);

            yield break;
        }
        
        private IEnumerator UnityEventDelayedAsync(UnityAction action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
