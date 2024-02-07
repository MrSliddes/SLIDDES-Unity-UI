using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionColor : Transition
    {
        public float FadeDuration => colorBlockGraphic.fadeDuration;
        public Graphic TargetGraphic
        {
            get
            {
                return targetGraphic;
            }
            set
            {
                targetGraphic = value;
            }
        }
        public TMP_Text TextField
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
            }
        }

        [SerializeField] private Graphic targetGraphic;
        [SerializeField] private TMP_Text textField;

        [Header("Graphic")]
        [SerializeField] private ColorBlock colorBlockGraphic = new ColorBlock() 
        {  
            normalColor = Color.white,
            highlightedColor = new Color(111 / 255f, 111 / 255f, 111 / 255f),
            pressedColor = new Color(26 / 255, 26 / 255, 26 / 255 ),
            selectedColor = new Color(111 / 255f, 111 / 255f, 111 / 255f),
            disabledColor = new Color(96 / 255f, 96 / 255f, 96 / 255f),
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        [Header("Text")]
        [SerializeField] private ColorBlock colorBlockText = new ColorBlock()
        {
            normalColor = new Color(26 / 255, 26 / 255, 26 / 255),
            highlightedColor = new Color(230 / 255f, 230 / 255f, 230 / 255f),
            pressedColor = Color.white,
            selectedColor = new Color(230 / 255f, 230 / 255f, 230 / 255f),
            disabledColor = new Color(128 / 255f, 128 / 255f, 128 / 255f),
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        private Navigation navigation;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour);
            this.navigation = (Navigation)objects[0];

            // Crossfade is multiplied against the default set color of the graphic. So we have to set it to white first to
            // be able to set our colors from the codeblock
            targetGraphic.color = Color.white;
            textField.color = Color.white;

            InteractableChanged(navigation.Interactable);
            navigation.onInteractable.AddListener(InteractableChanged);
        }

        public override void Enter()
        {
            base.Enter();
            targetGraphic.CrossFadeColor(colorBlockGraphic.highlightedColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField.CrossFadeColor(colorBlockText.highlightedColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        public override void Exit()
        {
            base.Exit();
            targetGraphic.CrossFadeColor(colorBlockGraphic.normalColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField.CrossFadeColor(colorBlockText.normalColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        public override void PointerDown()
        {
            base.PointerDown();
            targetGraphic.CrossFadeColor(colorBlockGraphic.pressedColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField.CrossFadeColor(colorBlockText.pressedColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        public override void PointerUp()
        {
            base.PointerUp();
            targetGraphic.CrossFadeColor(colorBlockGraphic.highlightedColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField.CrossFadeColor(colorBlockText.highlightedColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        private void InteractableChanged(bool value)
        {
            if(navigation.Interactable)
            {
                targetGraphic.CrossFadeColor(colorBlockGraphic.normalColor, 0, true, true);
                textField.CrossFadeColor(colorBlockText.normalColor, 0, true, true);
            }
            else
            {
                targetGraphic.CrossFadeColor(colorBlockGraphic.disabledColor, 0, true, true);
                textField.CrossFadeColor(colorBlockText.disabledColor, 0, true, true);
            }
        }
    }
}
