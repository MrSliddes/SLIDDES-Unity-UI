using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    /// <summary>
    /// Get a reference to the inputManager (handy for UnityEvents)
    /// </summary>
    public class InputManagerHandler : MonoBehaviour
    {
        public void UpdatePlayers()
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.UpdatePlayers();
            }
        }

        public void LoadPlayersInputActionAssets()
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.LoadPlayersInputActionAssets();
            }
        }

        public void SavePlayersInputActionAssetRebinds()
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.SavePlayersInputActionAssetRebinds();
            }
        }

        public void SetCurrentInputDevice(string inputDeviceName)
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.SetCurrentInputDevice(inputDeviceName);
            }
        }

        public void ResetAllInputActionAssetRebinds()
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.ResetAllInputActionAssetRebinds();
            }
        }

        public void ResetCurrentPlayerInputActionAssetRebinds()
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.ResetCurrentPlayerInputActionAssetRebinds();
            }
        }

        /// <summary>
        /// Disable all event system input but the current one
        /// </summary>
        public void SetMultiplayerEventSystemInputSolo()
        {
            if(InputManager.Instance != null)
            {
                InputManager.SetMultiplayerEventSystemInputSolo();
            }
        }

        /// <summary>
        /// Enable all event system inputs
        /// </summary>
        public void SetMultiplayerEventSystemInputAll()
        {
            if(InputManager.Instance != null)
            {
                InputManager.SetMultiplayerEventSystemInputAll();
            }
        }

        public void PlayersSwitchActionMap(string mapNameOrID)
        {
            if(InputManager.Instance != null)
            {
                InputManager.PlayersSwitchActionMap(mapNameOrID);
            }
        }
    }
}
