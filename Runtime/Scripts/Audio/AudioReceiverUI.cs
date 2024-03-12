using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Audio/AudioReceiverUI")]
    public class AudioReceiverUI : MonoBehaviour
    {
        [SerializeField] private Callback[] callbacks;
        [SerializeField] private bool showDebug;

        private Dictionary<string, UnityEvent> callbackDictionary = new Dictionary<string, UnityEvent>();

        private void Awake()
        {
            SetupDictionary();
        }

        public void Receive(string audioEvent)
        {
            if(!isActiveAndEnabled) return;

            if(callbackDictionary.ContainsKey(audioEvent))
            {
                callbackDictionary[audioEvent]?.Invoke();
            }
            else
            {
                if(showDebug) Debug.LogWarning($"[AudioReceiver] No callback found for {audioEvent}");
            }
        }

        private void SetupDictionary()
        {
            callbackDictionary.Clear();

            foreach(var item in callbacks)
            {
                callbackDictionary.Add(item.audioEvent, item.onCallback);
            }
        }

        [System.Serializable]
        public class Callback
        {
            public string audioEvent;
            public UnityEvent onCallback;
        }
    }
}
