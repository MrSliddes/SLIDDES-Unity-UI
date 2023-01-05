using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI.Navigator
{
    /// <summary>
    /// A menu for UI navigator. Only 1 instance of NavigatorMenu can be opend at a time. For multiple use NavigatorSubMenu
    /// </summary>
    [System.Serializable]
    [AddComponentMenu("SLIDDES/UI/Navigator Menu")]
    public class NavigatorMenu : Navigator
    {
        public override void Open()
        {
            if(NavigatorManager.CurrentMenu != this)
            {
                NavigatorManager.Open(this);
                return;
            }

            base.Open();
        }

        public override void Close()
        {
            if(NavigatorManager.CurrentMenu == this)
            {
                NavigatorManager.Instance.currentMenu = null;
            }

            base.Close();
        }
    }
}