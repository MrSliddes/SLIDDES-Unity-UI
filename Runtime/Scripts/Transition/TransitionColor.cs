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
        public CanvasGroup CanvasGroup
        {
            get
            {
                return canvasGroup;
            }
            set
            {
                canvasGroup = value;
            }
        }

        [SerializeField] private Graphic targetGraphic;
        [SerializeField] private TMP_Text textField;
        [SerializeField] private CanvasGroup canvasGroup;

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

        [Header("Canvas Group")]
        [SerializeField] private float fadeTime = 0.25f;
        [SerializeField] private bool fadeDelayBySiblingIndex;

        private Navigation navigation;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour);
            var n = monoBehaviour as Navigation;
            if(n != null) this.navigation = n;

            // ! IMPORTANT COLOR FUCKERY !
            // Crossfade is multiplied against the default set color of the graphic. So we have to set it to white first to
            // be able to set our colors from the codeblock
            //if(targetGraphic != null) targetGraphic.color = Color.white;
            //if(textField != null) textField.color = Color.white;
                        
            if(navigation != null)
            {
                //InteractableChanged(navigation.Interactable);
                navigation.onInteractable.AddListener(InteractableChanged);
            }    
            else
            {
                //InteractableChanged(true);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            // Reset base colors for canvasGroup
            if(targetGraphic != null) targetGraphic.color = Color.white;
            if(textField != null) textField.color = Color.white;
            if(navigation != null)
            {
                InteractableChanged(navigation.Interactable);
            }
            else
            {
                InteractableChanged(true);
            }
            FadeIn();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            InteractableChanged(false);
        }

        public override void Enter()
        {
            base.Enter();
            targetGraphic?.CrossFadeColor(colorBlockGraphic.highlightedColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField?.CrossFadeColor(colorBlockText.highlightedColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        public override void Exit()
        {
            base.Exit();
            targetGraphic?.CrossFadeColor(colorBlockGraphic.normalColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField?.CrossFadeColor(colorBlockText.normalColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        public override void PointerDown()
        {
            base.PointerDown();
            targetGraphic?.CrossFadeColor(colorBlockGraphic.pressedColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField?.CrossFadeColor(colorBlockText.pressedColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        public override void PointerUp()
        {
            base.PointerUp();
            targetGraphic?.CrossFadeColor(colorBlockGraphic.highlightedColor * colorBlockGraphic.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
            textField?.CrossFadeColor(colorBlockText.highlightedColor * colorBlockText.colorMultiplier, colorBlockGraphic.fadeDuration, true, true);
        }

        private void InteractableChanged(bool value)
        {
            if(value)
            {
                targetGraphic?.CrossFadeColor(colorBlockGraphic.normalColor, colorBlockGraphic.fadeDuration, true, true);
                textField?.CrossFadeColor(colorBlockText.normalColor, colorBlockText.fadeDuration, true, true);
            }
            else
            {
                targetGraphic?.CrossFadeColor(colorBlockGraphic.disabledColor, colorBlockGraphic.fadeDuration, true, true);
                textField?.CrossFadeColor(colorBlockText.disabledColor, colorBlockGraphic.fadeDuration, true, true);
            }

            if(textField != null) textField.SetMaterialDirty();
        }

        private void FadeIn()
        {
            if(canvasGroup != null)
            {
                monoBehaviour.StopCoroutine(FadeInAsync());
                monoBehaviour.StartCoroutine(FadeInAsync());
            }
        }

        private IEnumerator FadeInAsync()
        {
            float timer = 0;
            float time = fadeTime;
            canvasGroup.alpha = 0;
            if(fadeDelayBySiblingIndex)
            {
                int index = Mathf.Clamp(canvasGroup.transform.GetSiblingIndex(), 1, 99);
                yield return new WaitForSecondsRealtime(fadeTime * index);
            }
            while(true)
            {
                timer += Time.unscaledDeltaTime;
                canvasGroup.alpha = TransitionEasing.EaseInCubic(timer / time);
                if(timer >= time)
                {
                    canvasGroup.alpha = 1;
                    break;
                }
                yield return null;
            }
        }
    }
}
