using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.UIElements.Experimental;

namespace SLIDDES.UI
{
    /// <summary>
    /// Collection of all transitions
    /// </summary>
    [System.Serializable]
    public class Transitions
    {
        public bool Initalized { get; private set; }

        [SerializeField] private Transition.Type types;

        [SerializeField] private TransitionAnimation animation;
        [SerializeField] private TransitionAudio audio;
        [SerializeField] private TransitionColor color;
        [SerializeField] private TransitionProcedural procedural;
        [SerializeField] private TransitionRendering rendering;
        [SerializeField] private TransitionSprite sprite;
        [SerializeField] private TransitionText text;

        private List<Transition> activeTransitions = new List<Transition>();

        public void Initialize(MonoBehaviour monoBehaviour)
        {
            if(Initalized)
            {
                Debug.LogWarning("Transitions is already initialized!");
                return;
            }

            if(types.HasFlag(Transition.Type.procedural))
            {
                activeTransitions.Add(procedural);
            }
            if(types.HasFlag(Transition.Type.color))
            {
                activeTransitions.Add(color);
            }
            if(types.HasFlag(Transition.Type.animation))
            {
                activeTransitions.Add(animation);
            }
            if(types.HasFlag(Transition.Type.sprite))
            {
                activeTransitions.Add(sprite);
            }
            if(types.HasFlag(Transition.Type.text))
            {
                activeTransitions.Add(text);
            }
            if(types.HasFlag(Transition.Type.rendering))
            {
                activeTransitions.Add(rendering);
            }
            if(types.HasFlag(Transition.Type.audio))
            {
                activeTransitions.Add(audio);
            }

            foreach(Transition transition in activeTransitions)
            {
                transition.Initialize(monoBehaviour);
            }

            Initalized = true;
        }

        public void OnEnable()
        {
            foreach(Transition transition in activeTransitions)
            {
                transition.OnEnable();
            }
        }

        public void OnDisable() 
        { 
            foreach (Transition transition in activeTransitions)
            {
                transition.OnDisable();
            }
        }

        public void Enter()
        {
            foreach(Transition transition in activeTransitions)
            {
                transition.Enter();
            }
        }

        public void Update()
        {
            foreach(Transition transition in activeTransitions)
            {
                transition.Update();
            }
        }

        public void Exit()
        {
            foreach(Transition transition in activeTransitions)
            {
                transition.Exit();
            }
        }

        public void PointerDown()
        {
            foreach(Transition transition in activeTransitions)
            {
                transition.PointerDown();
            }
        }

        public void PointerUp()
        {
            foreach(Transition transition in activeTransitions)
            {
                transition.PointerUp();
            }
        }
    }
}
