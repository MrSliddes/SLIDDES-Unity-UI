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
        public void SetSelectedGameObject(GameObject selected)
        {
            if(EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(selected);
            }
        }

        public void UnsetSelectedGameObject()
        {
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
            InputManager.SetMultiplayerEventSystemInputSolo();
        }

        /// <summary>
        /// Enable all event system inputs
        /// </summary>
        public void SetMultiplayerEventSystemInputAll()
        {
            InputManager.SetMultiplayerEventSystemInputAll();
        }
    }
}
