using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Buttons/SliderSDS")]
    public class SliderSDS : ButtonSDS, IDragHandler, IInitializePotentialDragHandler
    {
        public bool ValueIsReversed
        {
            get
            {
                return direction == Direction.rightToLeft || direction == Direction.topToBottom;
            }
        }
        public bool ValueIsInt
        {
            get
            {
                return valueIsInt;
            }
            set
            {
                if(SetStruct(ref valueIsInt, value)) 
                {
                    SetNormalizedValue(this.value);
                    UpdateSlider();
                }
            }
        }
        public float MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                if(SetStruct(ref minValue, value))
                {
                    UpdateSlider();
                }
            }
        }
        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                if(SetStruct(ref maxValue, value))
                {
                    UpdateSlider();
                }
            }
        }
        public float Value
        {
            get
            {
                return valueIsInt ? Mathf.RoundToInt(value) : value;
            }
            set
            {
                value = Mathf.Clamp(value, MinValue, MaxValue);
                if(this.value != value)
                {
                    this.value = value;
                    onValueChanged.Invoke(this.value);
                    onNormalizedValueChanged.Invoke(NormalizedValue);
                    UpdateSlider();
                }
            }
        }
        public float NormalizedValue
        {
            get
            {
                if(Mathf.Approximately(MinValue, MaxValue)) return 0;
                return Mathf.InverseLerp(MinValue, MaxValue, value);
            }
            set
            {
                Value = Mathf.Lerp(MinValue, MaxValue, value);                
            }
        }
        public Axis Axis
        {
            get
            {
                return (direction == Direction.leftToRight || direction == Direction.rightToLeft) ? Axis.horizontal : Axis.vertical;
            }
        }

        public Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if(SetStruct(ref direction, value))
                {
                    UpdateSlider();
                }
            }
        }

        [SerializeField] private Direction direction;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue = 1;
        [SerializeField] private float value;
        [SerializeField] private bool valueIsInt;
        [SerializeField] private float stepSize = 0.1f;


        [SerializeField] private RectTransform filler;
        [SerializeField] private RectTransform handle;

        [SerializeField] private bool triggerValueChangedOnStart;
        public UnityEvent<float> onValueChanged;
        public UnityEvent<float> onNormalizedValueChanged;

        RectTransform handleContainerRect;
        RectTransform fillerContainerRect;
        private Image fillerImage;
        private Vector2 offset = Vector2.zero;
#pragma warning disable 649
        private DrivenRectTransformTracker drivenRectTransformTracker;
#pragma warning restore 649

        protected override void Awake()
        {
            base.Awake();
            fillerImage = filler.GetComponent<Image>();
            fillerContainerRect = filler.parent.GetComponent<RectTransform>();
            handleContainerRect = handle.parent.GetComponent<RectTransform>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateSlider();
        }

        protected override void OnDisable()
        {
            drivenRectTransformTracker.Clear();
            base.OnDisable();
        }

        protected override void Start()
        {
            base.Start();
            if(triggerValueChangedOnStart)
            {
                onValueChanged.Invoke(value);
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if(!IsActive()) return;

            UpdateSlider();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            //base.OnDidApplyAnimationProperties();
            UpdateSlider();
        }

        public virtual void SetNormalizedValue(float value)
        {
            NormalizedValue = value;
        }

        public virtual void SetNormalizedValueNoNotify(float value)
        {
            this.value = Mathf.Lerp(MinValue, MaxValue, value);
            UpdateSlider();
        }

        public virtual void SetValueNoNotify(float value)
        {
            this.value = value;
            UpdateSlider();
        }

        public virtual void SetDirection(Direction direction, bool includeRectLayouts)
        {
            Axis oldAxis = this.Axis;
            bool oldReverse = ValueIsReversed;
            Direction = direction;

            if(!includeRectLayouts) return;

            if(Axis != oldAxis)
            {
                RectTransformUtility.FlipLayoutAxes(transform as RectTransform, true, true);
            }

            if(ValueIsReversed != oldReverse)
            {
                RectTransformUtility.FlipLayoutOnAxis(transform as RectTransform, (int)Axis, true, true);
            }
        }

        public virtual void UpdateSlider()
        {
            Vector2 anchorMin = Vector2.zero;
            Vector2 anchorMax = Vector2.one;

            drivenRectTransformTracker.Clear();

            // Update filler
            if(fillerContainerRect != null)
            {

                if(fillerImage != null && fillerImage.type == Image.Type.Filled)
                {
                    fillerImage.fillAmount = NormalizedValue;
                }
                else
                {
                    if(ValueIsReversed)
                    {
                        anchorMin[(int)Axis] = 1 - NormalizedValue;
                    }
                    else
                    {
                        anchorMax[(int)Axis] = NormalizedValue;
                    }
                }

                filler.anchorMin = anchorMin;
                filler.anchorMax = anchorMax;
            }

            // Update handle
            if(handleContainerRect != null)
            {
                drivenRectTransformTracker.Add(this, handle, DrivenTransformProperties.Anchors);

                anchorMin = Vector2.zero;
                anchorMax = Vector2.one;

                anchorMin[(int)Axis] = anchorMax[(int)Axis] = (ValueIsReversed ? (1 - NormalizedValue) : NormalizedValue);
                
                handle.anchorMin = anchorMin;
                handle.anchorMax = anchorMax;
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if(!CanDrag(eventData)) return;

            UpdateDrag(eventData);
        }

        protected override void OnPointerDown(PointerEventData eventData)
        {
            if(!CanDrag(eventData)) return;

            base.OnPointerDown(eventData);

            offset = Vector2.zero;
            if(handleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(handle, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
            {
                Vector2 localMousePosition;
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(handle, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out localMousePosition))
                {
                    offset = localMousePosition;
                }
            }
            else
            {
                // Outside slider handle, jump to this point
                UpdateDrag(eventData);
            }
        }

        protected override void OnMove(AxisEventData eventData)
        {
            if(!IsActive() || !Interactable)
            {
                base.OnMove(eventData);
                return;
            }

            switch(eventData.moveDir)
            {
                case MoveDirection.Up:
                    if(Axis == Axis.vertical)
                    {
                        Value = (ValueIsReversed ? value - stepSize : value + stepSize);
                        return;
                    }
                    break;
                case MoveDirection.Down:
                    if(Axis == Axis.vertical)
                    {
                        Value = (ValueIsReversed ? value + stepSize : value - stepSize);
                    }
                    break;
                case MoveDirection.Left:
                    if(Axis == Axis.horizontal)
                    {
                        float v = (ValueIsReversed ? value + stepSize : value - stepSize);
                        Value = v;
                    }                    
                    break;
                case MoveDirection.Right:
                    if(Axis == Axis.horizontal)
                    {
                        Value = (ValueIsReversed ? value - stepSize : value + stepSize);
                    }
                    break;
                case MoveDirection.None:
                    break;
                default:
                    break;
            }
            base.OnMove(eventData);
        }

        protected override Navigation GetNavigationUp()
        {
            if(Axis == Axis.vertical) // no normal compare check implemented yet
            {
                return null;
            }

            return base.GetNavigationUp();
        }

        protected override Navigation GetNavigationDown()
        {
            if(Axis == Axis.vertical) // no normal compare check implemented yet
            {
                return null;
            }

            return base.GetNavigationDown();
        }

        protected override Navigation GetNavigationLeft()
        {
            if(Axis == Axis.horizontal && NormalizedValue > 0)
            {
                return null;
            }

            return base.GetNavigationLeft();
        }

        protected override Navigation GetNavigationRight()
        {
            if(Axis == Axis.horizontal && NormalizedValue < 1)
            {
                return null;
            }

            return base.GetNavigationRight();
        }

        private bool CanDrag(PointerEventData eventData)
        {
            if(eventData == null) return false;
            return IsActive() && Interactable && eventData.button == PointerEventData.InputButton.Left;
        }

        private void UpdateDrag(PointerEventData eventData)
        {
            Camera camera = eventData.pressEventCamera;

            RectTransform clickRect = handleContainerRect ?? fillerContainerRect;
            if(clickRect != null && clickRect.rect.size[(int)Axis] > 0)
            {
                Vector2 position = Vector2.zero;
                if(!GetRelativeMousePositionForDrag(eventData, ref position)) return;

                Vector2 localCursor;
                if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(clickRect, position, camera, out localCursor)) return;

                localCursor -= clickRect.rect.position;

                float val = Mathf.Clamp01((localCursor - offset)[(int)Axis] / clickRect.rect.size[(int)Axis]);
                NormalizedValue = (ValueIsReversed ? 1f - val : val);
            }
        }

        /// 

        private bool GetRelativeMousePositionForDrag(PointerEventData eventData, ref Vector2 position)
        {
#if UNITY_EDITOR
            position = eventData.position;
#else
            int pressDisplayIndex = eventData.pointerPressRaycast.displayIndex;
            var relativePosition = RelativeMouseAtScaled(eventData.position);
            int currentDisplayIndex = (int)relativePosition.z;

            // Discard events on a different display.
            if (currentDisplayIndex != pressDisplayIndex)
                return false;

            // If we are not on the main display then we must use the relative position.
            position = pressDisplayIndex != 0 ? (Vector2)relativePosition : eventData.position;
#endif
            return true;
        }

        /// <summary>
        /// A version of Display.RelativeMouseAt that scales the position when the main display has a different rendering resolution to the system resolution.
        /// By default, the mouse position is relative to the main render area, we need to adjust this so it is relative to the system resolution
        /// in order to correctly determine the position on other displays.
        /// </summary>
        /// <returns></returns>
        private Vector3 RelativeMouseAtScaled(Vector2 position)
        {
#if !UNITY_EDITOR && !UNITY_WSA
            // If the main display is now the same resolution as the system then we need to scale the mouse position. (case 1141732)
            if (Display.main.renderingWidth != Display.main.systemWidth || Display.main.renderingHeight != Display.main.systemHeight)
            {
                // The system will add padding when in full-screen and using a non-native aspect ratio. (case UUM-7893)
                // For example Rendering 1920x1080 with a systeem resolution of 3440x1440 would create black bars on each side that are 330 pixels wide.
                // we need to account for this or it will offset our coordinates when we are not on the main display.
                var systemAspectRatio = Display.main.systemWidth / (float)Display.main.systemHeight;

                var sizePlusPadding = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight);
                var padding = Vector2.zero;
                if (Screen.fullScreen)
                {
                    var aspectRatio = Screen.width / (float)Screen.height;
                    if (Display.main.systemHeight * aspectRatio < Display.main.systemWidth)
                    {
                        // Horizontal padding
                        sizePlusPadding.x = Display.main.renderingHeight * systemAspectRatio;
                        padding.x = (sizePlusPadding.x - Display.main.renderingWidth) * 0.5f;
                    }
                    else
                    {
                        // Vertical padding
                        sizePlusPadding.y = Display.main.renderingWidth / systemAspectRatio;
                        padding.y = (sizePlusPadding.y - Display.main.renderingHeight) * 0.5f;
                    }
                }

                var sizePlusPositivePadding = sizePlusPadding - padding;

                // If we are not inside of the main display then we must adjust the mouse position so it is scaled by
                // the main display and adjusted for any padding that may have been added due to different aspect ratios.
                if (position.y < -padding.y || position.y > sizePlusPositivePadding.y ||
                     position.x < -padding.x || position.x > sizePlusPositivePadding.x)
                {
                    var adjustedPosition = position;

                    if (!Screen.fullScreen)
                    {
                        // When in windowed mode, the window will be centered with the 0,0 coordinate at the top left, we need to adjust so it is relative to the screen instead.
                        adjustedPosition.x -= (Display.main.renderingWidth - Display.main.systemWidth) * 0.5f;
                        adjustedPosition.y -= (Display.main.renderingHeight - Display.main.systemHeight) * 0.5f;
                    }
                    else
                    {
                        // Scale the mouse position to account for the black bars when in a non-native aspect ratio.
                        adjustedPosition += padding;
                        adjustedPosition.x *= Display.main.systemWidth / sizePlusPadding.x;
                        adjustedPosition.y *= Display.main.systemHeight / sizePlusPadding.y;
                    }

                    var relativePos = Display.RelativeMouseAt(adjustedPosition);

                    // If we are not on the main display then return the adjusted position.
                    if (relativePos.z != 0)
                        return relativePos;
                }

                // We are using the main display.
                return new Vector3(position.x, position.y, 0);
            }
#endif
            return Display.RelativeMouseAt(position);
        }

        private bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if(EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }

        private bool SetClass<T>(ref T currentValue, T newValue) where T : class
        {
            if((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return false;

            currentValue = newValue;
            return true;
        }

        ///

        #region Interfaces

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        #endregion
    }
}
