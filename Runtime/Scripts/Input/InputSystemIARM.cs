using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    /// <summary>
    /// Monobehaviour wrapper class for InputActionReferenceMultiplayer
    /// </summary>
    public class InputSystemIARM : MonoBehaviour
    {
        [SerializeField] private InputActionReferenceMultiplayer iarm;

        public UnityEvent onCallback;

        private void Awake()
        {
            iarm.Callback = x =>
            {
                onCallback?.Invoke();
            };
        }

        private void OnEnable()
        {
            iarm.Enable();
        }

        private void OnDisable()
        {
            iarm.Disable();
        }
    }
}
