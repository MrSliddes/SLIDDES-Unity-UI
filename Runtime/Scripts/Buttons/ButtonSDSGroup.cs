using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Buttons/ButtonSDSGroup")]
    public class ButtonSDSGroup : MonoBehaviour
    {
        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
                UpdateButtons();
            }
        }

        [SerializeField] private bool interactable = true;
        [SerializeField] private bool ignoreParentGroups;

        private void OnEnable()
        {
            UpdateButtons();
        }

        private void OnValidate()
        {
            Interactable = interactable;
        }

        public void UpdateButtons()
        {
            ButtonSDS[] buttons = GetComponentsInChildren<ButtonSDS>(true);

            for (int i = 0; i < buttons.Length; i++)
            {
                ButtonSDS button = buttons[i];
                ButtonSDSGroup group = button.GetComponent<ButtonSDSGroup>();
                if (group != null && group.ignoreParentGroups)
                {
                    continue;
                }
                button.Interactable = interactable;
            }
        }
    }
}
