using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class ApplicationVersionToText : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<TMP_Text>().text = $"v{Application.version}";
        }
    }
}
