using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/EventSystemHandler")]
    public class EventSystemHandler : MonoBehaviour
    {
        public static GameObject SelectedGameObject { get; private set; }

        public EventSystem EventSystem
        {
            get
            {
                if(eventSystem != null) return eventSystem;
                return EventSystem.current;
            }
            set
            {
                eventSystem = value;
            }
        }

        private EventSystem eventSystem;

        /// <summary>
        /// Here to show enable in inspector
        /// </summary>
        private void OnEnable() { }

        public void SetSelectedGameObject(GameObject selected)
        {
            if(!enabled) return;

            SelectedGameObject = selected;

            if(EventSystem != null)
            {
                EventSystem.SetSelectedGameObject(selected);
            }
        }

        public void UnsetSelectedGameObject()
        {
            if(!enabled) return;

            if(EventSystem != null )
            {
                EventSystem.SetSelectedGameObject(null);
            }
        }

        /// <summary>
        /// Disable all event system input but the current one
        /// </summary>
        public void SetMultiplayerEventSystemInputSolo()
        {
            if(!enabled) return;

            InputManager.SetMultiplayerEventSystemInputSolo();
        }

        /// <summary>
        /// Enable all event system inputs
        /// </summary>
        public void SetMultiplayerEventSystemInputAll()
        {
            if(!enabled) return;

            InputManager.SetMultiplayerEventSystemInputAll();
        }

        /// <summary>
        /// Set this handlers event system
        /// </summary>
        /// <param name="gameObject"></param>
        public void SetEventSystemHandlerEventSystem(GameObject gameObject)
        {
            if (gameObject == null)
            {
                eventSystem = null;
                return;
            }
            eventSystem = gameObject.GetComponent<EventSystem>();
        }
    }
}
