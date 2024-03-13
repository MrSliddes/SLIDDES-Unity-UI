using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private bool onMenuOpenInputSolo;
        [SerializeField] private bool dontDestroyOnLoad;

        private void Awake()
        {
            if(dontDestroyOnLoad) DontDestroyOnLoad(transform.GetComponentInParent<Canvas>().gameObject);
        }

        public void Open()
        {
            if(onMenuOpenInputSolo)
            {
                InputManager.SetPlayerInputSolo();
            }
        }

        public void Close()
        {
            if(onMenuOpenInputSolo)
            {
                InputManager.SetPlayerInputAll();
            }
        }
    }
}
