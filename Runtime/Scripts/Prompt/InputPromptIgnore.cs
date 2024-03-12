using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI
{
    /// <summary>
    /// Attach this component to a TMP_Text script to ignore changes coming from InputPromptManager
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Input Prompt/Input Prompt Ignore")]
    [RequireComponent(typeof(TMP_Text))]
    public class InputPromptIgnore : MonoBehaviour, IInputPrompt
    {
        bool IInputPrompt.IgnoreByManager()
        {
            return true;
        }
    }
}
