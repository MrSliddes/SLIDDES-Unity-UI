using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionProcedural : Transition
    {
        public float TimePointerDown => timePointerDown;
        public Graphic TargetGraphic
        {
            get
            {
                return targetGraphic;
            }
            set
            {
                targetGraphic = value;
            }
        }

        [SerializeField] private Graphic targetGraphic;
        [SerializeField] private Transform siblingTransform;

        [Header("OnEnable")]
        [SerializeField] private bool transformOnEnable = true;
        [SerializeField] private float timeOnEnable = 0.25f;
        [SerializeField] private bool onEnableDelayBySiblingIndex;
        [SerializeField] private Vector3 onEnableLocalPosition;
        [SerializeField] private Vector3 onEnableLocalEulerRotation;
        [SerializeField] private Vector3 onEnableLocalScale = Vector3.one;

        [Header("Select")]
        [SerializeField] private bool transformOnSelect = true;
        [SerializeField] private float timeToSelect = 0.1f;
        [SerializeField] private Vector3 selectLocalPosition;
        [SerializeField] private Vector3 selectLocalEulerRotation;
        [SerializeField] private Vector3 selectLocalScale = new Vector3(1.1f, 1.1f, 1.1f);

        [Header("Hovering")]
        [SerializeField] private bool transformOnHover = true;
        [SerializeField] private float transitionToHoverTime = 0.1f;
        [SerializeField] private AnimationCurve hoveringLocalScaleAnimationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1.1f), new Keyframe(0.5f, 1f), new Keyframe(1, 1.1f) });

        [Header("Deselect")]
        [SerializeField] private bool transformOnDeselect = true;
        [SerializeField] private float timeToDeselect = 0.1f;
        [SerializeField] private Vector3 deselectLocalPosition;
        [SerializeField] private Vector3 deselectLocalEulerRotation;
        [SerializeField] private Vector3 deselectLocalScale = new Vector3(1, 1, 1);

        [Header("Pointer Down")]
        [SerializeField] private bool transformOnPointerDown = true;
        [SerializeField] private float timePointerDown = 0.1f;
        [SerializeField] private Vector3 pointerDownLocalPosition;
        [SerializeField] private Vector3 pointerDownLocalEulerRotation;
        [SerializeField] private Vector3 pointerDownLocalScale = new Vector3(0.8f, 0.8f, 0.8f);

        [Header("Pointer Up")]
        [SerializeField] private bool transformOnPointerUp = true;
        [SerializeField] private float timePointerUp = 0.2f;
        [SerializeField] private Vector3 pointerUpLocalPosition;
        [SerializeField] private Vector3 pointerUpLocalEulerRotation;
        [SerializeField] private Vector3 pointerUpLocalScale = new Vector3(1f, 1f, 1f);
        [SerializeField] private AnimationCurve pointerUpLocalScaleAnimationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1f), new Keyframe(0.5f, 1.5f), new Keyframe(1, 1f) });

        [Header("Tilt")]
        [SerializeField] private bool useTilt;
        [SerializeField] private float tiltSpeed = 1;
        [SerializeField] private float tiltAmountManual = 1;
        [SerializeField] private float tiltAmountAutomatic = 1;
        [SerializeField] private float tiltScaleManual = 0.2f;
        [SerializeField] private float tiltScaleAutomatic = 1;

        private Vector3 interruptedLocalPosition;
        private Vector3 interruptedLocalEulerRotation;
        private Vector3 interruptedLocalScale;
        private Vector3 pointerPosition;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour, objects);
            if(siblingTransform == null) siblingTransform = monoBehaviour.transform;
        }

        protected override IEnumerator OnEnableAsync()
        {
            if(onEnableDelayBySiblingIndex)
            {
                int index = Mathf.Clamp(siblingTransform.GetSiblingIndex(), 1, 99);
                yield return new WaitForSecondsRealtime(timeOnEnable * index);
            }

            if(transformOnEnable)
            {
                while(transitionTimer < timeOnEnable)
                {
                    float t = TransitionEasing.EaseOutCubic(transitionTimer / timeOnEnable);
                    targetGraphic.transform.localPosition = Vector3.Lerp(onEnableLocalPosition, deselectLocalPosition, t);
                    targetGraphic.transform.localRotation = Quaternion.Euler(Vector3.Lerp(onEnableLocalEulerRotation, deselectLocalEulerRotation, t));
                    targetGraphic.transform.localScale = Vector3.Lerp(onEnableLocalScale, deselectLocalScale, t);
                    transitionTimer += Time.unscaledDeltaTime;
                    yield return null;
                }

                // Done
                // Deadset
                targetGraphic.transform.localPosition = deselectLocalPosition;
                targetGraphic.transform.localRotation = Quaternion.Euler(deselectLocalEulerRotation);
                targetGraphic.transform.localScale = deselectLocalScale;
            }

            transitionTimer = 0;
            yield break;
        }

        protected override IEnumerator EnterAsync()
        {
            if(transformOnSelect)
            {
                while(transitionTimer < timeToSelect)
                {
                    float t = transitionTimer / timeToSelect;
                    targetGraphic.transform.localPosition = Vector3.Lerp(deselectLocalPosition, selectLocalPosition, t);
                    targetGraphic.transform.localRotation = Quaternion.Euler(Vector3.Lerp(deselectLocalEulerRotation, selectLocalEulerRotation, t));
                    targetGraphic.transform.localScale = Vector3.Lerp(deselectLocalScale, selectLocalScale, t);
                    transitionTimer += Time.unscaledDeltaTime;
                    yield return null;
                }

                // Done
                // Deadset
                targetGraphic.transform.localPosition = selectLocalPosition;
                targetGraphic.transform.localRotation = Quaternion.Euler(selectLocalEulerRotation);
                targetGraphic.transform.localScale = selectLocalScale;
            }

            transitionTimer = 0;
            Update();
            yield break;
        }

        protected override IEnumerator UpdateAsync()
        {
            transitionTimer = 0;
            interruptedLocalScale = targetGraphic.transform.localScale;

            bool transitionedToHover = false;

            while(transformOnHover || useTilt)
            {
                if(transformOnHover)
                {
                    if(!transitionedToHover)
                    {
                        while(transitionTimer < transitionToHoverTime)
                        {
                            yield return null;
                            float t = transitionTimer / transitionToHoverTime;
                            transitionTimer += Time.unscaledDeltaTime;
                            targetGraphic.transform.localScale = Vector3.Lerp(interruptedLocalScale, hoveringLocalScaleAnimationCurve.Evaluate(0) * Vector3.one, t);
                        }
                        transitionTimer = 0;
                        transitionedToHover = true;
                    }

                    targetGraphic.transform.localScale = Vector3.one * hoveringLocalScaleAnimationCurve.Evaluate(transitionTimer);
                    transitionTimer += Time.unscaledDeltaTime;
                    if(transitionTimer > hoveringLocalScaleAnimationCurve[hoveringLocalScaleAnimationCurve.length - 1].time)
                    {
                        transitionTimer = 0;
                    }
                }

                if(useTilt)
                {
                    UpdateTilt();
                }

                yield return null;
            }
        }

        protected override IEnumerator ExitAsync()
        {
            if(useTilt) // TODO - make sure this just lerps well instead of zeroing it
            {
                targetGraphic.transform.localEulerAngles = Vector3.zero;
            }

            interruptedLocalPosition = targetGraphic.transform.localPosition;
            interruptedLocalEulerRotation = targetGraphic.transform.localEulerAngles;
            interruptedLocalScale = targetGraphic.transform.localScale;

            if(transformOnDeselect)
            {
                while(transitionTimer < timeToDeselect)
                {
                    float t = transitionTimer / timeToDeselect;
                    targetGraphic.transform.localPosition = Vector3.Lerp(interruptedLocalPosition, deselectLocalPosition, t);
                    targetGraphic.transform.localRotation = Quaternion.Euler(Vector3.Lerp(interruptedLocalEulerRotation, deselectLocalEulerRotation, t));
                    targetGraphic.transform.localScale = Vector3.Lerp(interruptedLocalScale, deselectLocalScale, t);
                    transitionTimer += Time.unscaledDeltaTime;
                    yield return null;
                }

                // Done
                // Deadset
                targetGraphic.transform.localPosition = deselectLocalPosition;
                targetGraphic.transform.localRotation = Quaternion.Euler(deselectLocalEulerRotation);
                targetGraphic.transform.localScale = deselectLocalScale;
            }

            transitionTimer = 0;
            yield break;
        }

        public override void PointerDown()
        {
            if(useTilt) // TODO - make sure this just lerps well instead of zeroing it
            {
                targetGraphic.transform.localEulerAngles = Vector3.zero;
            }

            interruptedLocalPosition = targetGraphic.transform.localPosition;
            interruptedLocalEulerRotation = targetGraphic.transform.localEulerAngles;
            interruptedLocalScale = targetGraphic.transform.localScale;

            base.PointerDown();
        }

        protected override IEnumerator PointerDownAsync()
        {
            if(transformOnPointerDown)
            {
                while(transitionTimer < timePointerDown)
                {
                    float t = transitionTimer / timePointerDown;
                    targetGraphic.transform.localPosition = Vector3.Lerp(interruptedLocalPosition, pointerDownLocalPosition, t);
                    targetGraphic.transform.localRotation = Quaternion.Euler(Vector3.Lerp(interruptedLocalEulerRotation, pointerDownLocalEulerRotation, t));
                    targetGraphic.transform.localScale = Vector3.Lerp(interruptedLocalScale, pointerDownLocalScale, t);
                    transitionTimer += Time.unscaledDeltaTime;
                    yield return null;
                }

                // Done
                // Deadset
                targetGraphic.transform.localPosition = pointerDownLocalPosition;
                targetGraphic.transform.localRotation = Quaternion.Euler(pointerDownLocalEulerRotation);
                targetGraphic.transform.localScale = pointerDownLocalScale;
            }

            transitionTimer = 0;
            yield break;
        }

        public override void PointerUp()
        {
            interruptedLocalPosition = targetGraphic.transform.localPosition;
            interruptedLocalEulerRotation = targetGraphic.transform.localEulerAngles;
            interruptedLocalScale = targetGraphic.transform.localScale;

            base.PointerUp();
        }

        protected override IEnumerator PointerUpAsync()
        {
            if(transformOnPointerUp)
            {
                while(transitionTimer < timePointerUp)
                {
                    float t = transitionTimer / timePointerUp;
                    targetGraphic.transform.localPosition = Vector3.Lerp(interruptedLocalPosition, pointerUpLocalPosition, t);
                    targetGraphic.transform.localRotation = Quaternion.Euler(Vector3.Lerp(interruptedLocalEulerRotation, pointerUpLocalEulerRotation, t));
                    targetGraphic.transform.localScale = Vector3.Lerp(interruptedLocalScale, pointerUpLocalScale, t) * pointerUpLocalScaleAnimationCurve.Evaluate(t);
                    transitionTimer += Time.unscaledDeltaTime;
                    yield return null;
                }

                // Done
                // Deadset
                targetGraphic.transform.localPosition = pointerUpLocalPosition;
                targetGraphic.transform.localRotation = Quaternion.Euler(pointerUpLocalEulerRotation);
                targetGraphic.transform.localScale = pointerUpLocalScale;
            }

            transitionTimer = 0;
            Update();
            yield break;
        }

        public override void PointerMove(PointerEventData eventData)
        {
            base.PointerMove(eventData);
            pointerPosition = eventData.position;
        }

        private void UpdateTilt()
        {
            if(!useTilt) return;
            if(state == State.pointerDown || state == State.pointerUp) return;

            float sine = Mathf.Sin(Time.time) * (IsHovering ? tiltScaleManual : tiltScaleAutomatic);
            float cosine = Mathf.Cos(Time.time) * (IsHovering ? tiltScaleManual : tiltScaleAutomatic);

            Vector3 offset = targetGraphic.transform.position - pointerPosition;
            float tiltX = IsHovering ? ((offset.y * -1) * tiltAmountManual) : 0;
            float tiltY = IsHovering ? ((offset.x) * tiltAmountManual) : 0;
            float tiltZ = targetGraphic.transform.eulerAngles.z;

            float lerpX = Mathf.LerpAngle(targetGraphic.transform.eulerAngles.x, tiltX + (sine * tiltAmountAutomatic), tiltSpeed * Time.deltaTime);
            float lerpY = Mathf.LerpAngle(targetGraphic.transform.eulerAngles.y, tiltY + (cosine * tiltAmountAutomatic), tiltSpeed * Time.deltaTime);
            //float lerpZ = Mathf.LerpAngle(targetGraphic.transform.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

            targetGraphic.transform.eulerAngles = new Vector3(lerpX, lerpY, 0);
        }
    }

    //[System.Serializable]
    //public class TransformBlock
    //{
    //    public bool usePosition;
    //    public bool positionIsLocal = true;
    //    public Vector3 position;
    //    public bool useEulerRotation;
    //    public bool eulerRotationIsLocal = true;
    //    public Vector3 eulerRotation;
    //    public bool useScale;
    //    public bool scaleIsLocal = true;
    //    public Vector3 scale;

    //    public TransformBlock(Vector3 position, Vector3 eulerRotation, Vector3 scale)
    //    {
    //        this.position = position;
    //        this.eulerRotation = eulerRotation;
    //        this.scale = scale;
    //    }
    //}
}
