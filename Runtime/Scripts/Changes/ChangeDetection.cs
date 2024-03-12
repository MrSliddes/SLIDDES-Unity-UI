using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    /// <summary>
    /// Base class for detecting change
    /// </summary>
    public abstract class ChangeDetection : MonoBehaviour
    {
        [SerializeField] private bool showDebug;

        public UnityEvent onFoundChange;
        public UnityEvent onLostChange;
        public UnityEvent onRevertChanges;
        public UnityEvent onApplyChanges;

        protected bool isChanged;
        protected UnappliedChangesUI unappliedChangesUI;

        private readonly string debugPrefix = "[ChangeDetection]";

        protected virtual void Awake()
        {
            unappliedChangesUI = FindObjectOfType<UnappliedChangesUI>();
        }

        public virtual void ApplyChanges()
        {
            isChanged = false;
            onApplyChanges?.Invoke();
            onLostChange?.Invoke();
        }

        public virtual void RevertChanges()
        {
            isChanged = false;
            onRevertChanges?.Invoke();
            onLostChange?.Invoke();
        }

        public virtual void LostChanges()
        {
            isChanged = false;
            onLostChange?.Invoke();
        }

        protected virtual void FindChanges()
        {
            isChanged = false;
            // Find changes

            // If no changes found
            // onLostChange?.Invoke();
        }

        protected virtual void FoundChange(int changeIndex = -1)
        {
            if(showDebug) Debug.Log($"{debugPrefix} Found Change");
            isChanged = true;
            onFoundChange?.Invoke();
        }        
    }
}
