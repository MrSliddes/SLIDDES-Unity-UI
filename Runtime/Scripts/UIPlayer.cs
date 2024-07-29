using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(InputSystemUIInputModule))]
    [AddComponentMenu("SLIDDES/UI/UI Player")]
    public class UIPlayer : MonoBehaviour
    {
        [Tooltip("Sync all event systems")]
        [SerializeField] private bool syncEventSystems = true;

        private MultiplayerEventSystem multiplayerEventSystem;
        private InputSystemUIInputModule inputSystemUIInputModule;

        private void Awake()
        {
            inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
            multiplayerEventSystem = GetComponent<MultiplayerEventSystem>();
        }

        private void OnEnable()
        {
            inputSystemUIInputModule.move.action.performed += OnMove;
            if(multiplayerEventSystem != null)
            {
                GameObject g = EventSystem.current.currentSelectedGameObject != null ? EventSystem.current.currentSelectedGameObject : EventSystemHandler.SelectedGameObject;
                multiplayerEventSystem.SetSelectedGameObject(g);
            }

            if(syncEventSystems)
            {
                InputManager.OnLastPlayerInputPressChanged += OnLastPlayerInputPressedChanged;
            }

            if(EventSystem.current == null)
            {
                EventSystem.current = multiplayerEventSystem;
            }
        }

        private void OnDisable()
        {
            inputSystemUIInputModule.move.action.performed -= OnMove;

            if(syncEventSystems)
            {
                InputManager.OnLastPlayerInputPressChanged -= OnLastPlayerInputPressedChanged;
            }
        }

        private void OnLastPlayerInputPressedChanged(InputManager.Player player)
        {
            if(player == null) return;
            if(!multiplayerEventSystem.enabled) return;

            // If changed to this player, set selected to event system
            if(player.PlayerInput.uiInputModule == inputSystemUIInputModule)
            {
                GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
                EventSystem.current = multiplayerEventSystem;
                EventSystem.current.SetSelectedGameObject(selectedObject);
            }
        }

        /// <summary>
        /// Callback from InputSystemUIInputModule move
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            if(EventSystem.current == multiplayerEventSystem && EventSystem.current.currentSelectedGameObject == null)
            {
                // Find a navigation regain gameobject;
                NavigationRegainer navigationRegainer = FindObjectOfType<NavigationRegainer>();
                if(navigationRegainer != null)
                {
                    navigationRegainer.Regain();
                }
            }
        }
    }
}
