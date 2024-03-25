using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI.ThemeSwitching
{
    [System.Serializable]
    public class Theme
    {
        public string name;
        public ColorBinding[] colorBindings;
        public FontBinding[] fontBindings;

        public string GetColorBindingName(Color32 color32)
        {
            ColorBinding colorBinding = colorBindings.FirstOrDefault(x => x.value.Equals(color32));
            if(colorBinding == null)
            {
                return "";
            }
            return colorBinding.name;
        }

        public Color32 GetColorBindingValue(string name)
        {
            return colorBindings.FirstOrDefault(x => x.name == name).value;
        }
    }

    [System.Serializable]
    public class Binding<T0>
    {
        public string name;
        public T0 value;
    }

    [System.Serializable]
    public class ColorBinding : Binding<Color32>
    {        

    }

    [System.Serializable]
    public class FontBinding : Binding<TMP_FontAsset>
    {
        
    }
}
