using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    [AddComponentMenu("SLIDDES/UI/Audio/AudioSenderUI")]
    public class AudioSenderUI : MonoBehaviour
    {
        [Tooltip("Auto assigned if left empty")]
        [SerializeField] private AudioReceiverUI audioReceiverUI;

        private void Awake()
        {
            if(audioReceiverUI == null) audioReceiverUI = GetComponentInParent<AudioReceiverUI>();
        }

        public void Send(string audioEvent)
        {
            audioReceiverUI.Receive(audioEvent);
        }
    }
}
