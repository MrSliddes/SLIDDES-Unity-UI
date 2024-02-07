using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        [Header("Select")]
        [SerializeField] private float timeToSelect = 0.1f;
        [SerializeField] private Vector3 selectLocalPosition;
        [SerializeField] private Vector3 selectLocalEulerRotation;
        [SerializeField] private Vector3 selectLocalScale = new Vector3(1.1f, 1.1f, 1.1f);

        [Header("Hovering")]
        [SerializeField] private float transitionToHoverTime = 0.1f;
        [SerializeField] private AnimationCurve hoveringLocalScaleAnimationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1.1f), new Keyframe(0.5f, 1f), new Keyframe(1, 1.1f) });

        [Header("Deselect")]
        [SerializeField] private float timeToDeselect = 0.1f;
        [SerializeField] private Vector3 deselectLocalPosition;
        [SerializeField] private Vector3 deselectLocalEulerRotation;
        [SerializeField] private Vector3 deselectLocalScale = new Vector3(1, 1, 1);

        [Header("Pointer Down")]
        [SerializeField] private float timePointerDown = 0.1f;
        [SerializeField] private Vector3 pointerDownLocalPosition;
        [SerializeField] private Vector3 pointerDownLocalEulerRotation;
        [SerializeField] private Vector3 pointerDownLocalScale = new Vector3(0.8f, 0.8f, 0.8f);

        [Header("Pointer Up")]
        [SerializeField] private float timePointerUp = 0.2f;
        [SerializeField] private Vector3 pointerUpLocalPosition;
        [SerializeField] private Vector3 pointerUpLocalEulerRotation;
        [SerializeField] private Vector3 pointerUpLocalScale = new Vector3(1f, 1f, 1f);
        [SerializeField] private AnimationCurve pointerUpLocalScaleAnimationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1f), new Keyframe(0.5f, 1.5f), new Keyframe(1, 1f) });

        private Vector3 interruptedLocalPosition;
        private Vector3 interruptedLocalEulerRotation;
        private Vector3 interruptedLocalScale;


        protected override IEnumerator EnterAsync()
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

            transitionTimer = 0;
            Update();
            yield break;
        }

        protected override IEnumerator UpdateAsync()
        {
            transitionTimer = 0;
            interruptedLocalScale = targetGraphic.transform.localScale;

            while(transitionTimer < transitionToHoverTime)
            {
                yield return null;
                float t = transitionTimer / transitionToHoverTime;
                transitionTimer += Time.unscaledDeltaTime;
                targetGraphic.transform.localScale = Vector3.Lerp(interruptedLocalScale, hoveringLocalScaleAnimationCurve.Evaluate(0) * Vector3.one, t);
            }
            transitionTimer = 0;

            while(true)
            {
                yield return null;
                targetGraphic.transform.localScale = Vector3.one * hoveringLocalScaleAnimationCurve.Evaluate(transitionTimer);
                transitionTimer += Time.unscaledDeltaTime;
                if(transitionTimer > hoveringLocalScaleAnimationCurve[hoveringLocalScaleAnimationCurve.length - 1].time)
                {
                    transitionTimer = 0;
                }
            }
        }

        protected override IEnumerator ExitAsync()
        {
            interruptedLocalPosition = targetGraphic.transform.localPosition;
            interruptedLocalEulerRotation = targetGraphic.transform.localEulerAngles;
            interruptedLocalScale = targetGraphic.transform.localScale;

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

            transitionTimer = 0;
            yield break;
        }

        public override void PointerDown()
        {
            interruptedLocalPosition = targetGraphic.transform.localPosition;
            interruptedLocalEulerRotation = targetGraphic.transform.localEulerAngles;
            interruptedLocalScale = targetGraphic.transform.localScale;

            base.PointerDown();
        }

        protected override IEnumerator PointerDownAsync()
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

            transitionTimer = 0;
            Update();
            yield break;
        }
    }
}
