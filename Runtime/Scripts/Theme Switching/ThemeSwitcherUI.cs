using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI.ThemeSwitching
{
    public class ThemeSwitcherUI : MonoBehaviour
    {
        [SerializeField] private Theme defaultTheme;

        [SerializeField] private string themeOnStart;
        [SerializeField] private Theme[] themes;

        // Start is called before the first frame update
        void Start()
        {
            SetTheme(themeOnStart);
        }

        public void SetTheme(string themeName)
        {
            Theme theme = themes.FirstOrDefault(x => x.name == themeName);
            if(theme == null)
            {
                Debug.LogError($"No theme with name {themeName} found!", this);
                return;
            }

            SwitchAll(theme);
        }

        private void SwitchAll(Theme theme)
        {
            SwitchColors(theme);
        }

        private void SwitchColors(Theme theme)
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();
            foreach(Transform t in transforms)
            {
                IThemeSwitcherColor themeSwitcherColor = t.GetComponent<IThemeSwitcherColor>();
                if(themeSwitcherColor != null)
                {
                    themeSwitcherColor.Switch(theme.colorBindings);
                }
                else
                {
                    TMP_Text tmpText = t.GetComponent<TMP_Text>();
                    if(tmpText != null)
                    {
                        tmpText.color = theme.GetColorBindingValue(defaultTheme.GetColorBindingName(tmpText.color));
                    }
                }
            }
        }
    }
}
