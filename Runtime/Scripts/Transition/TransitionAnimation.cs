using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionAnimation : Transition
    {
        public float NormalizedTransitionDuration => normalizedTransitionDuration;

        [SerializeField] private Animator animator;
        [SerializeField] private string normalTrigger = "Normal";
        [SerializeField] private string hoverTrigger = "Hover";
        [SerializeField] private string pressedTrigger = "Pressed";
        [SerializeField] private string selectTrigger = "Selected";
        [SerializeField] private string disabledTrigger = "Disabled";
        [SerializeField] private float normalizedTransitionDuration = 0.1f;

        private Navigation navigation;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour, objects);

            this.navigation = monoBehaviour.GetComponent<Navigation>();
            InteractableChanged(navigation.Interactable);
            navigation.onInteractable.AddListener(InteractableChanged);
        }

        public override void Enter()
        {
            base.Enter();
            animator.CrossFade(hoverTrigger, normalizedTransitionDuration);
        }

        public override void Exit()
        {
            base.Exit();
            animator.CrossFade(normalTrigger, normalizedTransitionDuration);
        }

        public override void PointerDown()
        {
            base.PointerDown();
            animator.CrossFade(pressedTrigger, normalizedTransitionDuration);
        }

        public override void PointerUp()
        {
            base.PointerUp();
            animator.CrossFade(hoverTrigger, normalizedTransitionDuration);
        }

        private void InteractableChanged(bool value)
        {
            if(navigation.Interactable)
            {
                animator.CrossFade(normalTrigger, normalizedTransitionDuration);
            }
            else
            {
                animator.CrossFade(disabledTrigger, normalizedTransitionDuration);
            }
        }
    }
}
