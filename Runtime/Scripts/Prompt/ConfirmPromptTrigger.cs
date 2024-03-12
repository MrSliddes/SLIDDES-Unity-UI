using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    public class ConfirmPromptTrigger : MonoBehaviour
    {
        [Tooltip("Time that countdowns to 0 & invokes the cancel. -1 to have unlimited time")]
        [SerializeField] private float autoCancelTime = -1;
        [TextArea(1, 10)]
        [SerializeField] private string promptMessage = "Are you sure you want to ...";

        public UnityEvent onCancel;
        public UnityEvent onConfirm;
        [SerializeField] private ConfirmPrompt confirmPrompt;

        public void Trigger()
        {
            Trigger(null);
        }

        public void Trigger(GameObject invoker)
        {
            confirmPrompt.Show(invoker, autoCancelTime, promptMessage, () => onCancel?.Invoke(), () => onConfirm?.Invoke());
        }
    }
}
