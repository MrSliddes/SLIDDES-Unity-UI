using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Buttons/SideButton")]
    public class SideButton : ButtonSDS
    {
        public int Index => currentIndex;
        public Axis Axis 
        { 
            get 
            {
                return (direction == Direction.leftToRight || direction == Direction.rightToLeft) ? Axis.horizontal : Axis.vertical; 
            } 
        }

        [SerializeField] private Direction direction = Direction.leftToRight;
        [SerializeField] private TMP_Text textField;
        [SerializeField] private int currentIndex;
        [SerializeField] private bool selectOnStart;
        [SerializeField] private List<Option> options;

        public UnityEvent<int> onSelectOption;

        protected override void OnEnable()
        {
            base.OnEnable();

            for(int i = 0; i < options.Count; i++)
            {
                Navigation navigation = options[i].navigation;
                if(navigation == null) continue;

                int index = i;
                options[i].navigation.OnActionSelect += () => SideSelect(index);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            for(int i = 0; i < options.Count; i++)
            {
                Navigation navigation = options[i].navigation;
                if(navigation == null) continue;

                int index = i;
                options[i].navigation.OnActionSelect -= () => SideSelect(index);
            }
        }

        protected override void Start()
        {
            base.Start();
            SideSelect(currentIndex);
        }

        public void Next()
        {
            SideSelect(true);
        }

        public void Previous()
        {
            SideSelect(false);
        }

        public void SideSelect(bool next = true)
        {
            currentIndex += next ? 1 : -1;
            if(currentIndex < 0) currentIndex = options.Count - 1;
            else if(currentIndex >= options.Count) currentIndex = 0;

            SideSelect(currentIndex);
        }

        public void SideSelect(int index)
        {
            currentIndex = index;

            onSelectOption.Invoke(currentIndex);
            if(options[currentIndex].navigation != null)
            {
                options[currentIndex].navigation.Select();
            }
            options[currentIndex].onSelect?.Invoke();
            options[currentIndex].onSelectIndex?.Invoke(currentIndex);
            if(textField != null)
            {
                textField.text = string.IsNullOrEmpty(options[currentIndex].displayLabel) ? options[currentIndex].label : options[currentIndex].displayLabel;
            }
        }

        public void SideSelectCurrentIndex()
        {
            SideSelect(currentIndex);
        }

        public void SideSelectNoNotify(int index)
        {
            currentIndex = index;
            if(textField != null)
            {
                textField.text = string.IsNullOrEmpty(options[currentIndex].displayLabel) ? options[currentIndex].label : options[currentIndex].displayLabel;
            }
        }

        public void SideSelectWithLabel(string label)
        {
            Option op = options.FirstOrDefault(x => x.label == label);
            if(op == null) return;

            int index = options.IndexOf(op);
            SideSelect(index);
        }

        public void SideSelectNoNotifyWithLabel(string label)
        {
            Option op = options.FirstOrDefault(x => x.label == label);
            if(op == null) return;

            int index = options.IndexOf(op);
            SideSelectNoNotify(index);
        }

        public void SideSelectWithLabel(int value)
        {
            SideSelectWithLabel(value.ToString());
        }

        public void SideSelectNoNotifyWithLabel(int value)
        {
            SideSelectNoNotifyWithLabel(value.ToString());
        }

        public void SetSideOptions(string[] optionLabels)
        {
            SetSideOptions(optionLabels, null, null);
        }

        public void SetSideOptions(string[] optionLabels, UnityAction actionOnSelect = null, UnityAction<int> actionOnSelectIndex = null)
        {
            this.options.Clear();
            for(int i = 0; i < optionLabels.Length; i++)
            {
                Option option = new Option(optionLabels[i], actionOnSelect, actionOnSelectIndex);
                options.Add(option);
            }
        }

        protected override Navigation GetNavigationUp()
        {
            if(NavigateUpTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(NavigationMode == Mode.Automatic && Axis == Axis.vertical && NavigateUp == null)
            {
                return null;
            }
            return base.GetNavigationUp();
        }

        protected override Navigation GetNavigationDown()
        {
            if(NavigateDownTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(NavigationMode == Mode.Automatic && Axis == Axis.vertical && NavigateDown == null)
            {
                return null;
            }
            return base.GetNavigationDown();
        }

        protected override Navigation GetNavigationLeft()
        {
            if(NavigateLeftTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(NavigationMode == Mode.Automatic && Axis == Axis.horizontal && NavigateLeft == null)
            {
                return null;
            }
            return base.GetNavigationLeft();
        }

        protected override Navigation GetNavigationRight()
        {
            if(NavigateRightTriggerSubmit)
            {
                OnSubmit(null);
                return null;
            }

            if(NavigationMode == Mode.Automatic && Axis == Axis.horizontal && NavigateRight == null)
            {
                return null;
            }
            return base.GetNavigationRight();
        }

        protected override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            SideSelect(currentIndex);
        }

        [System.Serializable]
        public class Option
        {
            public string label;
            public string displayLabel;
            public Navigation navigation;
            public UnityEvent onSelect;
            public UnityEvent<int> onSelectIndex;

            public Option() { }

            public Option(string label, UnityAction actionOnSelect, UnityAction<int> actionOnSelectIndex)
            {
                this.label = label;
                onSelect?.AddListener(actionOnSelect);
                onSelectIndex?.AddListener(actionOnSelectIndex);
            }
        }
    }
}
