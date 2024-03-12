using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(SideButton))]
    public class ChangeDetectionSideButton : ChangeDetection
    {
        public UnityEvent<int> onRevertValue;
        public UnityEvent<int> onApplyValue;

        private int previousIndex;
        private int preApplyIndex;
        private SideButton sideButton;
        private UnityAction<int> actionOnSelectOption;

        protected override void Awake()
        {
            base.Awake();
            sideButton = GetComponent<SideButton>();

            actionOnSelectOption = x => FindChanges();
        }

        private void OnEnable()
        {
            sideButton.onSelectOption.AddListener(actionOnSelectOption);
            preApplyIndex = sideButton.Index;
            previousIndex = sideButton.Index;
        }

        private void OnDisable()
        {
            sideButton.onSelectOption.RemoveListener(actionOnSelectOption);
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            preApplyIndex = previousIndex;
            previousIndex = sideButton.Index;
            onApplyValue.Invoke(sideButton.Index);
        }

        public override void RevertChanges()
        {
            base.RevertChanges();
            sideButton.SideSelectNoNotify(preApplyIndex);
            previousIndex = preApplyIndex;
            onRevertValue.Invoke(preApplyIndex);
        }

        public override void LostChanges()
        {
            preApplyIndex = sideButton.Index;
            previousIndex = sideButton.Index;
            unappliedChangesUI.RemoveChangeDetection(this);
            base.LostChanges();
        }

        protected override void FindChanges()
        {
            base.FindChanges();

            if (previousIndex != sideButton.Index)
            {
                FoundChange(0);
            }

            if(!isChanged)
            {
                LostChanges();
            }
        }

        protected override void FoundChange(int changeIndex = -1)
        {
            base.FoundChange(changeIndex);
            preApplyIndex = previousIndex;

            unappliedChangesUI.AddChangeDetection(this);
        }
    }
}
