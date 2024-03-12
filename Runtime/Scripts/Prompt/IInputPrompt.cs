using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    public interface IInputPrompt
    {
        /// <summary>
        /// Ignore this by the input prompt manager?
        /// </summary>
        public bool IgnoreByManager() {  return false; }

        public virtual void OnSpriteAssetChange(TMP_SpriteAsset spriteAsset)
        {

        }        

        public virtual void OnSpriteAssetChange(string profileName)
        {

        }
    }
}
