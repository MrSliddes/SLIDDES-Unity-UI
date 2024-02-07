using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionSprite : Transition
    {
        [SerializeField] private Image image;

        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private Sprite pressedSprite;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite disabledSprite;

        private Sprite normalSprite;
        private Navigation navigation;

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour, objects);
            navigation = (Navigation)objects[0];

            normalSprite = image.sprite;

            InteractableChanged(navigation.Interactable);
            navigation.onInteractable.AddListener(InteractableChanged);
        }

        public override void Enter()
        {
            base.Enter();
            image.sprite = hoverSprite;
        }

        public override void Exit() 
        { 
            base.Exit();
            image.sprite = normalSprite;
        }

        public override void PointerDown()
        {
            base.PointerDown();
            image.sprite = pressedSprite;
        }

        public override void PointerUp()
        {
            base.PointerUp();
            image.sprite = hoverSprite;
        }

        private void InteractableChanged(bool value)
        {
            if(value)
            {
                image.sprite = normalSprite;
            }
            else
            {
                image.sprite = disabledSprite;
            }
        }
    }
}
