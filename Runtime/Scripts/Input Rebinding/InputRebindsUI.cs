using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    /// <summary>
    /// Generates input rebind UI elements
    /// </summary>
    public class InputRebindsUI : MonoBehaviour
    {
        [SerializeField] private bool setEventSystemSelectedOnGenerate = true;
        [SerializeField] private InputRebindUI.Content[] inputRebindUIContents;
        [SerializeField] private Transform inputRebindsUIParent;
        [SerializeField] private GameObject prefabInputRebindUI;
        [SerializeField] private TMP_Text controlBindingsTextField;

        public UnityEvent onGenerated;

        private readonly string debugPrefix = "[InputRebindsUI]";
        private Coroutine coroutineGenerate;
        private List<InputRebindUI> inputRebindUIs = new List<InputRebindUI>();
        private UnityAction<string> actionOnInputDeviceChange;
        private UnityAction<InputManager.Player> actionOnLastPlayerInputPressChanged;

        private void Awake()
        {
            actionOnInputDeviceChange = x => 
            { 
                if(!string.IsNullOrEmpty(x))
                {
                    Generate(); 
                }
            };
            actionOnLastPlayerInputPressChanged = x => 
            { 
                if(x != null)
                {
                    Generate(); 
                }
            };
        }

        private void OnEnable()
        {
            if(InputManager.Instance != null)
            {
                InputManager.OnInputDeviceNameChanged += actionOnInputDeviceChange;
                InputManager.OnLastPlayerInputPressChanged += actionOnLastPlayerInputPressChanged;
            }
            Generate();
        }

        private void OnDisable()
        {
            if(InputManager.Instance != null )
            {
                InputManager.OnInputDeviceNameChanged -= actionOnInputDeviceChange;
                InputManager.OnLastPlayerInputPressChanged -= actionOnLastPlayerInputPressChanged;
            }
        }

        private void OnDestroy()
        {
            if(InputManager.Instance != null)
            {
                InputManager.OnInputDeviceNameChanged -= actionOnInputDeviceChange;
                InputManager.OnLastPlayerInputPressChanged -= actionOnLastPlayerInputPressChanged;
            }
        }

        public void Generate()
        {
            if (this.isActiveAndEnabled)
            {
                if(InputManager.Instance == null)
                {
                    Debug.Log("Cannot generate as InputManger is null", this);
                    return;
                }

                if (coroutineGenerate != null) StopCoroutine(coroutineGenerate);
                coroutineGenerate = StartCoroutine(GenerateAsync());                
            }
        }

        private IEnumerator GenerateAsync()
        {
            if(InputManager.LastPlayerInputPress == null)
            {
                Debug.Log($"{debugPrefix} No player to generate for");
                yield break;
            }

            Debug.Log($"{debugPrefix} Generate input rebindings UI for {InputManager.LastPlayerInputPress.Index}");

            controlBindingsTextField.text = $"Control Bindings - Player {InputManager.LastPlayerInputPress.Index + 1}";

            // Clear old
            for(int i = inputRebindUIs.Count - 1; i >= 0; i--)
            {
                Destroy(inputRebindUIs[i].Container);
                inputRebindUIs.RemoveAt(i);
            }
            // Wait a frame to process destroying the gameobjects!
            yield return null;

            // Get input action references of current player that has control
            List<InputActionReference> references = new List<InputActionReference>();

            // Loop through all action maps
            foreach(InputActionMap actionMap in InputManager.LastPlayerInputPress.InputActionAsset.actionMaps)
            {
                // Loop through all actions in the map
                foreach(InputAction action in actionMap.actions)
                {
                    InputActionReference reference = ScriptableObject.CreateInstance<InputActionReference>();
                    reference.Set(action);
                    references.Add(reference);
                }
            }

            for(int i = 0; i < references.Count; i++)
            {
                InputActionReference item = references[i];
                // Check if inputActionReference can be created
                if(!InputManager.AbleToDisplayInputActionReference(item)) continue;

                GameObject g = Instantiate(prefabInputRebindUI, inputRebindsUIParent);
                InputRebindUI inputRebindUI = g.GetComponentInChildren<InputRebindUI>();
                if(inputRebindUI != null)
                {
                    inputRebindUIs.Add(inputRebindUI);

                    InputRebindUI.Content content = inputRebindUIContents.FirstOrDefault(x => x.inputActionNameReference == item.action.name && x.actionMapReference == item.action.actionMap.name);

                    inputRebindUI.Initialize(item, content);
                }
                else
                {
                    Debug.LogError($"{debugPrefix} No InputRebindUI Component found for prefabInputRebindUI!");
                }
            }

            yield return null;

            InputManager.ForceUpdateInputPrompts();

            yield return null;

            if(setEventSystemSelectedOnGenerate && inputRebindUIs.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(inputRebindUIs[0].RebindButton.gameObject);
            }

            onGenerated?.Invoke();
            yield break;
        }
    }
}
