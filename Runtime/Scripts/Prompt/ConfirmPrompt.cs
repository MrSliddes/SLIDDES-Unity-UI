using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SLIDDES.UI
{
    public class ConfirmPrompt : MonoBehaviour
    {
        [SerializeField] private TMP_Text promptTextField;
        [SerializeField] private TMP_Text autoCancelPromptTextField;
        [SerializeField] private ButtonSDS buttonCancel;
        [SerializeField] private ButtonSDS buttonConfirm;
        [SerializeField] private CanvasGroup canvasGroup;

        public UnityEvent onShow;
        public UnityEvent onHide;
        public UnityEvent onNoLastInvoker;

        private UnityAction actionCancel;
        private UnityAction actionConfirm;

        private Coroutine coroutineAutoCancel;
        private GameObject lastInvoker;

        // Start is called before the first frame update
        void Start()
        {
            Hide();
        }

        public void Show(GameObject invoker, float autoCancelTime, string promptMessage, UnityAction onCancel, UnityAction onConfirm)
        {
            buttonCancel.onPointerUp.RemoveListener(x => actionCancel.Invoke());
            buttonConfirm.onPointerUp.RemoveListener(x => actionConfirm.Invoke());

            actionCancel = onCancel;
            actionConfirm = onConfirm;
            lastInvoker = invoker;

            buttonCancel.onPointerUp.AddListener(x => actionCancel.Invoke());
            buttonConfirm.onPointerUp.AddListener(x => actionConfirm.Invoke());
            actionCancel += () => { if(coroutineAutoCancel != null) StopCoroutine(coroutineAutoCancel); };
            actionConfirm += () => { if(coroutineAutoCancel != null) StopCoroutine(coroutineAutoCancel); };

            promptTextField.text = promptMessage;

            buttonCancel.Interactable = true;
            buttonConfirm.Interactable = true;

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

            buttonConfirm.OnDeselect(null);
            EventSystem.current.SetSelectedGameObject(buttonCancel.gameObject);

            if(coroutineAutoCancel != null) StopCoroutine(coroutineAutoCancel);
            coroutineAutoCancel = StartCoroutine(AutoCancel(autoCancelTime));

            onShow?.Invoke();
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            buttonCancel.Interactable = false;
            buttonConfirm.Interactable = false;
            if(lastInvoker != null)
            {
                EventSystem.current.SetSelectedGameObject(lastInvoker);
            }
            else
            {
                onNoLastInvoker?.Invoke();
            }

            onHide?.Invoke();
        }

        private IEnumerator AutoCancel(float autoCancelTime)
        {
            if(autoCancelTime < 0)
            {
                autoCancelPromptTextField.text = "";
                yield break;
            }

            float timer = autoCancelTime;
            while(timer > 0)
            {
                autoCancelPromptTextField.text = $"Reverting changes in {(int)timer} second{((int)timer == 1 ? "" : "s")}";
                timer -= Time.unscaledDeltaTime;
                yield return null;
            }

            Hide();
            actionCancel?.Invoke();

            yield break;
        }
    }
}
