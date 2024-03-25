using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI.ThemeSwitching
{
    public interface IThemeSwitcher
    {
        
    }

    public interface IThemeSwitcherColor : IThemeSwitcher
    {
        public abstract void Switch(ColorBinding[] colorBindings);
    }

    public interface IThemeSwitcherFont : IThemeSwitcher
    {
        public abstract void Switch(FontBinding[] fontBindings);
    }
}
