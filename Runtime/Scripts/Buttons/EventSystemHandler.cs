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

        /// <summary>
        /// Here to show enable in inspector
        /// </summary>
        private void OnEnable() { }

        public void SetSelectedGameObject(GameObject selected)
        {
            if(!enabled) return;

            SelectedGameObject = selected;
            if(EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(selected);
            }
        }

        public void UnsetSelectedGameObject()
        {
            if(!enabled) return;

            if(EventSystem.current != null )
            {
                EventSystem.current.SetSelectedGameObject(null);
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
    }
}
