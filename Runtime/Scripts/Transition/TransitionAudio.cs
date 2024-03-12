using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    [System.Serializable]
    public class TransitionAudio : Transition
    {
        [SerializeField] private bool connectToAudioReceiverUI = true;
        [Tooltip("The audio receiver to send events to. If left null it gets auto assigned (if connectToAudioReceiverUI is true)")]
        [SerializeField] private AudioReceiverUI audioReceiverUI;

        [SerializeField] private string audioEventOnEnable = "onEnable";
        [SerializeField] private string audioEventOnDisable = "onDisable";
        [SerializeField] private string audioEventEnter = "enter";
        [SerializeField] private string audioEventUpdate = "update";
        [SerializeField] private string audioEventExit = "exit";
        [SerializeField] private string audioEventPointerDown = "pointerDown";
        [SerializeField] private string audioEventPointerUp = "pointerUp";

        public override void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            base.Initialize(monoBehaviour, objects);

            if(connectToAudioReceiverUI && audioReceiverUI == null)
            {
                audioReceiverUI = monoBehaviour.transform.GetComponentInParent<AudioReceiverUI>();
                if(audioReceiverUI == null)
                {
                    Debug.LogError($"[TransitionAudio] No AudioReceiverUI component found!");
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            SendAudioEvent(audioEventOnEnable);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            SendAudioEvent(audioEventOnDisable);
        }

        public override void Enter()
        {
            base.Enter();
            SendAudioEvent(audioEventEnter);
        }

        public override void Update()
        {
            base.Update();
            SendAudioEvent(audioEventUpdate);
        }

        public override void Exit()
        {
            base.Exit();
            SendAudioEvent(audioEventExit);
        }

        public override void PointerDown()
        {
            base.PointerDown();
            SendAudioEvent(audioEventPointerDown);
        }

        public override void PointerUp()
        {
            base.PointerUp();
            SendAudioEvent(audioEventPointerUp);
        }

        private void SendAudioEvent(string audioEvent)
        {
            if(audioReceiverUI == null)
            {
                Debug.LogWarning($"[TransitionAudio] No AudioReceiverUI component found! Tried sending audioEvent {audioEvent}");
                return;
            }

            audioReceiverUI.Receive(audioEvent);
        }
    }
}
