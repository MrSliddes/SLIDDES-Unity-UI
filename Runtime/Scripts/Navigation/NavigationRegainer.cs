using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    /// <summary>
    /// Attach this to a navigation gameobject for when navigation is lost it can be regained here
    /// </summary>
    [RequireComponent(typeof(Navigation))]
    public class NavigationRegainer : MonoBehaviour
    {
        public UnityEvent onRegain;

        public void Regain()
        {
            onRegain?.Invoke();
        }
    }
}
