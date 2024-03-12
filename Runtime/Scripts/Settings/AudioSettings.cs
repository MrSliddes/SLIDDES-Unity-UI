using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private bool loadOnAwake;
        [SerializeField] private Volume[] volumes;

        [Space]

        [SerializeField] private bool showDebug;

        private static readonly string savePrefix = "VideoSettings";
        private readonly string debugPrefix = "[VideoSettings]";
        private Volume focusedVolume;

        private void Awake()
        {
            if(loadOnAwake) LoadSettings();
        }

        public void LoadSettings()
        {
            for (int i = 0; i < volumes.Length; i++)
            {
                volumes[i].LoadSetting();
            }
        }

        public void SetFocusedVolume(string name)
        {
            Volume volume = volumes.FirstOrDefault(x => x.name == name);
            if(volume == null)
            {
                focusedVolume = null;
                return;
            }

            focusedVolume = volume;
        }

        public void SetFocusedVolumeValue(float value)
        {
            if(focusedVolume == null)
            {
                if(showDebug) Debug.LogWarning($"{debugPrefix} Focused volume was not set!");
                return;
            }

            value = Mathf.Clamp01(value);

            if(showDebug) Debug.Log($"{debugPrefix} Set focused volume {focusedVolume.name} value to {value}");            
            focusedVolume.SetValue(value);
        }

        private static string GetPlayerPrefKey(string key)
        {
            return $"{Application.productName}_{savePrefix}_{key}";
        }

        [System.Serializable]
        public class Volume
        {
            public string name;
            [Range(0f, 1f)]
            public float defaultValue = 0.5f;

            public UnityEvent<float> onSetValue;

            private float currentVolume;

            public void LoadSetting()
            {
                currentVolume = PlayerPrefs.GetFloat(GetPlayerPrefKey(name), defaultValue);
                SetValue(currentVolume);
            }

            public void SetValue(float value)
            {
                currentVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(GetPlayerPrefKey(name), currentVolume);
                onSetValue.Invoke(currentVolume);
            }
        }
    }
}
