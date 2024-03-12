using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(Selectable))]
    public class NavigationToSelectable : Navigation
    {
        private Selectable selectable;

        protected override void Awake()
        {
            base.Awake();
            selectable = GetComponent<Selectable>();
        }

        protected override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            selectable.Select();
        }
    }
}
