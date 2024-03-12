using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SLIDDES.UI
{
    public class TooltipSender : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public TooltipContent Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }

        [SerializeField] private TooltipContent content;

        private TooltipReceiver receiver;

        private void Awake()
        {
            receiver = GetComponentInParent<TooltipReceiver>();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            receiver.Receive(content);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            receiver.Receive(null);
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            receiver.Receive(content);
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            receiver.Receive(null);
        }
    }
}
