using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    public class Transitionable : MonoBehaviour
    {
        [SerializeField] private Transitions transitions;

        private void Awake()
        {
            transitions.Initialize(this);
        }

        private void OnEnable()
        {
            transitions.OnEnable();
        }

        private void OnDisable()
        {
            transitions.OnDisable();
        }
    }
}
