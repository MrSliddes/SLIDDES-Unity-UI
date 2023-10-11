using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : UIBehaviour, 
    IMoveHandler,
    IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    public Values values;
    public Events events;

    private Coroutine coroutineOnSelect;

	public void Pressed()
    {
        events.onPointerDown.Invoke(null);
    }

    private IEnumerator OnSelectEnumerator()
    {
        float time = 0;
        while(true)
        {
            time += Time.deltaTime;
            transform.localScale = values.scaleSelect * values.animCurveSelect.Evaluate(time);
            if(time >= values.animCurveSelect.keys[values.animCurveSelect.length - 1].time) time = 0;
            yield return null;
        }
    }

    public void HoverEnter()
    {
		transform.localScale = values.scaleSelect;
		if(coroutineOnSelect != null) StopCoroutine(coroutineOnSelect);
		coroutineOnSelect = StartCoroutine(OnSelectEnumerator());
        events.onHoverEnter.Invoke();
	}

    public void HoverExit()
    {
		if(coroutineOnSelect != null) StopCoroutine(coroutineOnSelect);
		transform.localScale = values.scaleDeselect;
        events.onHoverExit.Invoke();
	}

    #region Interfaces

    void IMoveHandler.OnMove(AxisEventData eventData)
    {
        events.onMove.Invoke(eventData);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        events.onPointerDown.Invoke(eventData);
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        events.onPointerUp.Invoke(eventData);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        HoverEnter();
        events.onPointerEnter.Invoke(eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        HoverExit();
        events.onPointerExit.Invoke(eventData);
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {        
        HoverEnter();
        events.onSelect.Invoke(eventData);
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        HoverExit();
        events.onDeselect.Invoke(eventData);
    }

    #endregion

    [System.Serializable]
    public class Events
    {
        public UnityEvent<AxisEventData> onMove;
        public UnityEvent<PointerEventData> onPointerDown;
        public UnityEvent<PointerEventData> onPointerUp;
        public UnityEvent<PointerEventData> onPointerEnter;
        public UnityEvent<PointerEventData> onPointerExit;
        public UnityEvent<BaseEventData> onSelect;
        public UnityEvent<BaseEventData> onDeselect;
        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;
    }

    [System.Serializable]
    public class Values
    {
        public Vector3 scaleSelect = new Vector3(1.01f, 1.01f, 1.01f);
        public Vector3 scaleDeselect = Vector3.one;
        [Tooltip("When selected the scaleSelect value is multiplied with this value over time")]
        public AnimationCurve animCurveSelect = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(0.5f, 1.1f), new Keyframe(1, 1) });
    }
}
