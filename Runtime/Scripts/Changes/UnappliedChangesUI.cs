using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    public class UnappliedChangesUI : MonoBehaviour
    {
        [SerializeField] private bool discardChangesOnDisable = true;
        [SerializeField] private TransitionColor transitionColor;

        public UnityEvent onHide;
        public UnityEvent onShow;
        public UnityEvent<bool> onToggle;
        public UnityEvent onApplyChanges;
        public UnityEvent onRevertChanges;

        private bool showingUI = true;
        private bool hasAppliedChanges;
        private bool isRevertingChanges;
        private List<ChangeDetection> changes = new List<ChangeDetection>();

        private void Awake()
        {
            transitionColor.Initialize(this);
        }

        private void OnEnable()
        {
            changes.Clear();
            HideUI();
        }

        private void OnDisable()
        {
            if(discardChangesOnDisable)
            {
                RevertChanges();
            }
        }

        public void AddChangeDetection(ChangeDetection changeDetection)
        {
            if(isRevertingChanges) return;

            if(hasAppliedChanges)
            {
                changes.Clear();
                hasAppliedChanges = false;
            }

            if(changes.Contains(changeDetection))
            {
                return;
            }

            changes.Add(changeDetection);
            ShowUI();
        }

        public void RemoveChangeDetection(ChangeDetection changeDetection)
        {
            if(isRevertingChanges) return;

            if(!changes.Contains(changeDetection)) return;

            changes.Remove(changeDetection);
            if(changes.Count <= 0) HideUI();
        }

        public void ApplyChanges()
        {
            for(int i = 0; i < changes.Count; i++)
            {
                changes[i].ApplyChanges();
            }

            hasAppliedChanges = true;
            HideUI();
            onApplyChanges?.Invoke();
        }

        public void RevertChanges()
        {
            // Set to true to prevent add/remove, cause list gets cleared after (+ prevents removing during reverting)
            isRevertingChanges = true;

            for(int i = 0; i < changes.Count; i++)
            {
                changes[i].RevertChanges();
            }
            changes.Clear();
            isRevertingChanges = false;

            hasAppliedChanges = false;
            HideUI();
            onRevertChanges?.Invoke();
        }

        public void LostChanges()
        {
            isRevertingChanges = true;

            for(int i = 0; i < changes.Count; i++)
            {
                changes[i].LostChanges();
            }
            changes.Clear();

            isRevertingChanges = false;

            HideUI();
        }

        private void ShowUI()
        {
            if(showingUI) return;
            showingUI = true;
            transitionColor.Enter();
            transitionColor.CanvasGroup.alpha = 1;
            onShow?.Invoke();
            onToggle?.Invoke(true);
        }

        public void HideUI()
        {
            if(!showingUI) return;
            showingUI = false;
            transitionColor.Exit();
            transitionColor.CanvasGroup.alpha = 0;
            onHide?.Invoke();
            onToggle?.Invoke(false);
        }
    }
}
