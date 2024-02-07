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
    [AddComponentMenu("SLIDDES/UI/Navigation/UI Navigation System")]
    public class NavigationSystem : MonoBehaviour
    {
        public UnityEvent<GameObject> onSelectedGameObjectChange;

        private GameObject selectedGameObject;
        private GameObject lastSelectedGameObject;

        private InputSystemUIInputModule inputSystemUIInputModule;

        private void Awake()
        {
            inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
        }

        private void OnEnable()
        {
            inputSystemUIInputModule.move.action.performed += OnMove;
        }

        private void OnDisable()
        {
            inputSystemUIInputModule.move.action.performed -= OnMove;
        }

        // Start is called before the first frame update
        void Start()
        {
            lastSelectedGameObject = EventSystem.current.firstSelectedGameObject;
        }

        // Update is called once per frame
        void Update()
        {
            CheckSelectedGameObject();
        }

        /// <summary>
        /// Check the event system selected gameobject
        /// </summary>
        private void CheckSelectedGameObject()
        {
            if(EventSystem.current.currentSelectedGameObject != selectedGameObject)
            {
                if(selectedGameObject != null)
                {
                    lastSelectedGameObject = selectedGameObject;
                }

                selectedGameObject = EventSystem.current.currentSelectedGameObject;
                onSelectedGameObjectChange.Invoke(selectedGameObject);
            }
        }

        /// <summary>
        /// Callback from InputSystemUIInputModule move
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            if(EventSystem.current.currentSelectedGameObject == null && lastSelectedGameObject != null)
            {
                Navigation navigation = lastSelectedGameObject.GetComponent<Navigation>();
                if(navigation != null) navigation.Select();
            }
        }
    }
}
