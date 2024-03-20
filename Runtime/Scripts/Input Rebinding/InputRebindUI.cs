using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SLIDDES.UI
{
    public class InputRebindUI : MonoBehaviour
    {
        public ButtonSDS RebindButton => rebindButtonSDS;
        public GameObject Container => container;

        [Tooltip("Destroy the input rebind when it can't provide a valid rebind")]
        [SerializeField] private bool destroyOnFailure;
        [SerializeField] private TMP_Text textField;
        [SerializeField] private ButtonSDS rebindButtonSDS;
        [SerializeField] private TMP_Text textInputBind;
        [SerializeField] private GameObject container;
        [SerializeField] private TooltipSender tooltipSender;

        public UnityEvent onRebindStart;
        public UnityEvent onRebindCancel;
        public UnityEvent onRebindSet;
        public UnityEvent onRebindComplete;

        private string bindingID;
        private Content contentOverride;
        private InputActionReference inputActionReference;
        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        public void Initialize(InputActionReference inputActionReference, Content contentOverride = null)
        {
            this.inputActionReference = inputActionReference;
            this.contentOverride = contentOverride;
            if(this.contentOverride != null)
            {
                if(!this.contentOverride.displayInputRebind)
                {
                    if(destroyOnFailure)
                    {
                        Destroy(container);
                    }
                    else
                    {
                        container.SetActive(false);
                    }
                    return;
                }

                if(tooltipSender != null) tooltipSender.Content = contentOverride.tooltipContent;
            }

            textField.text = inputActionReference.action.name;
            textInputBind.spriteAsset = InputManager.CurrentSpriteAsset;
            bindingID = InputManager.GetBindingID(inputActionReference);
            if(container == null) container = gameObject;
            UpdateBindingDisplay();
        }

        private void OnEnable()
        {
            rebindButtonSDS.onPointerUp.AddListener(x => StartRebind());
            InputSystem.onActionChange += OnActionChanged;
        }

        private void OnDisable()
        {
            rebindButtonSDS.onPointerUp.RemoveListener(x => StartRebind());
            DisposeRebindOperation();
            InputSystem.onActionChange -= OnActionChanged;
        }

        public void ResetRebinding()
        {
            if(!ResolveActionAndBinding(out var action, out var bindingIndex)) return;

            if(action.bindings[bindingIndex].isComposite)
            {
                // It's a composite. Remove overrides from part bindings.
                for(var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                {
                    action.RemoveBindingOverride(i);
                }
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }
            UpdateBindingDisplay();
        }   

        public void StartRebind()
        {
            if(!ResolveActionAndBinding(out InputAction inputAction, out int bindingIndex)) return;

            // If the binding is a composite, we need to rebind each part in turn.
            if(inputAction.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if(firstPartIndex < inputAction.bindings.Count && inputAction.bindings[firstPartIndex].isPartOfComposite)
                {
                    PerformRebind(inputAction, firstPartIndex, true);
                }
            }
            else
            {
                PerformRebind(inputAction, bindingIndex);
            }
        }

        public void UpdateBindingDisplay()
        {
            if(string.IsNullOrEmpty(bindingID))
            {
                // If binding ID was null or empty that means that no inputaction was found for this controlScheme
                Debug.LogWarning("[InputRebindUI] Binding ID was empty");
                if(destroyOnFailure)
                {
                    Destroy(container);
                }
                else
                {
                    container.SetActive(false);
                }
                return;
            }

            string actionMapNameDisplay = contentOverride != null ? contentOverride.actionMapNameDisplay : $"[{inputActionReference.action.actionMap.name}]";
            string inputActionNameDisplay = contentOverride != null ? contentOverride.inputActionNameDisplay : inputActionReference.action.name;
            textField.text = $"{actionMapNameDisplay} {inputActionNameDisplay}";

            string stringInputBind = InputManager.GetInputPrompt(inputActionReference);
            if(string.IsNullOrEmpty(stringInputBind))
            {
                Debug.LogWarning("[InputRebindUI] stringInputBind was empty");
                if(destroyOnFailure)
                {
                    Destroy(container);
                }
                else
                {
                    container.SetActive(false);                    
                }
                return;
            }
            textInputBind.text = stringInputBind;
        }

        private void DisposeRebindOperation()
        {
            rebindingOperation?.Dispose();
            rebindingOperation = null;
        }

        /// <summary>
        /// Return the action and binding index for the binding that is targeted by the component
        /// according to
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bindingIndex"></param>
        /// <returns></returns>
        private bool ResolveActionAndBinding(out InputAction inputAction, out int bindingIndex)
        {
            bindingIndex = -1;

            inputAction = inputActionReference?.action;
            if(inputAction == null) return false;

            if(string.IsNullOrEmpty(bindingID))
                return false;

            // Look up binding index.
            var bindingId = new System.Guid(bindingID);
            bindingIndex = inputAction.bindings.IndexOf(x => x.id == bindingId);
            if(bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{inputAction}'", this);
                return false;
            }

            return true;
        }

        private void PerformRebind(InputAction inputAction, int bindingIndex, bool allCompositeParts = false)
        {
            rebindingOperation?.Cancel(); // Will null out m_RebindOperation.
            inputAction.Disable(); // disable inputAction before rebinding

            DisposeRebindOperation();

            // Configure the rebind.
            rebindingOperation = inputAction.PerformInteractiveRebinding(bindingIndex).OnCancel(operation =>
            {
                onRebindCancel?.Invoke();
                DisposeRebindOperation();
                UpdateBindingDisplay();
                inputAction.Enable();
            })
            .OnComplete(operation =>
            {
                rebindingOperation?.Cancel(); // Will null out m_RebindOperation.
                onRebindSet?.Invoke();
                inputAction.Enable(); // enable input action again
                UpdateBindingDisplay();
                DisposeRebindOperation();


                // If there's more composite parts we should bind, initiate a rebind
                // for the next part.
                if(allCompositeParts)
                {
                    var nextBindingIndex = bindingIndex + 1;
                    if(nextBindingIndex < inputAction.bindings.Count && inputAction.bindings[nextBindingIndex].isPartOfComposite)
                    {
                        PerformRebind(inputAction, nextBindingIndex, true);
                    }
                }

                if(rebindingOperation == null)
                {
                    InputManager.Instance.SavePlayersInputActionAssetRebinds();
                    InputManager.ForceUpdateInputPrompts();
                    onRebindComplete?.Invoke();
                }
            });

            // If it's a part binding, show the name of the part in the UI.
            var partName = string.Empty;
            if(inputAction.bindings[bindingIndex].isPartOfComposite)
            {
                partName = $"Press '{inputAction.bindings[bindingIndex].name}'";
            }

            textInputBind.text = string.IsNullOrEmpty(partName) ? "Press New Input..." : partName;

            onRebindStart?.Invoke();

            rebindingOperation.Start();
        }

        // When the action system re-resolves bindings, we want to update our UI in response. While this will
        // also trigger from changes we made ourselves, it ensures that we react to changes made elsewhere. If
        // the user changes keyboard layout, for example, we will get a BoundControlsChanged notification and
        // will update our UI to reflect the current keyboard layout.
        private void OnActionChanged(object obj, InputActionChange change)
        {
            if(change != InputActionChange.BoundControlsChanged) return;

            var action = obj as InputAction;
            var actionMap = action?.actionMap ?? obj as InputActionMap;
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

            if(inputActionReference == null) return;
            if(inputActionReference.action == null) return;

            if(inputActionReference.action == action 
                || inputActionReference.action.actionMap == actionMap 
                || inputActionReference.action.actionMap.asset == actionAsset)
            {
                UpdateBindingDisplay();
            }
        }

        [System.Serializable]
        public class Content
        {
            public string inputActionNameReference;
            public string actionMapReference;
            public bool displayInputRebind = true;

            [Space]

            public string inputActionNameDisplay;
            public string actionMapNameDisplay;
            public TooltipContent tooltipContent;
        }
    }
}
