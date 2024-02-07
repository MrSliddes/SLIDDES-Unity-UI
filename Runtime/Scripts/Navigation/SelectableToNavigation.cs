using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    /// <summary>
    /// Go from Unity's selectable to SLIDDES Navigation
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Navigation/Selectable To Navigation")]
    [RequireComponent(typeof(Navigation))]
    public class SelectableToNavigation : Selectable
    {
        private Navigation nav;

        protected override void Awake()
        {
            base.Awake();
            nav = GetComponent<Navigation>();
        }

        public override void Select()
        {
            base.Select();
            nav.Select();
        }
    }
}
