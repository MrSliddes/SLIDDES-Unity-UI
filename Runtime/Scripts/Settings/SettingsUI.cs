using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private bool onMenuOpenInputSolo;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
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
