using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionText : Transition
    {
        public string DefaultText
        {
            get { return defaultText; }
            set { defaultText = value; }
        }

        [SerializeField] private TMP_Text textField;

        [SerializeField] private string hoverText;
        [SerializeField] private string pressedText;
        [SerializeField] private string selectedText;
        [SerializeField] private string disabledText;

        private string defaultText;
        private Navigation navigation;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour, objects);
            navigation = (Navigation)objects[0];

            defaultText = textField.text;
            InteractableChanged(navigation.Interactable);
            navigation.onInteractable.AddListener(InteractableChanged);
        }

        public override void Enter()
        {
            base.Enter();
            textField.text = hoverText;
        }

        public override void Exit()
        {
            base.Exit();
            textField.text = defaultText;
        }

        public override void PointerDown()
        {
            base.PointerDown();
            textField.text = pressedText;
        }

        public override void PointerUp()
        {
            base.PointerUp();
            textField.text = hoverText;
        }

        private void InteractableChanged(bool value)
        {
            if(value)
            {
                textField.text = defaultText;
            }
            else
            {
                textField.text = disabledText;
            }
        }
    }
}
