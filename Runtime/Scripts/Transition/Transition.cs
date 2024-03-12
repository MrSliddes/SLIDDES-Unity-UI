using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLIDDES.UI
{
    /// <summary>
    /// Base class for a transition
    /// </summary>
    public abstract class Transition
    {
        protected State state = State.exit;

        protected float transitionTimer;
        private Coroutine coroutine;
        protected MonoBehaviour monoBehaviour;

        public virtual void Initialize(MonoBehaviour monoBehaviour, params object[] objects)
        {
            this.monoBehaviour = monoBehaviour;
        }

        public virtual void OnEnable()
        {
            state = State.onEnable;
            transitionTimer = 0;
            if(coroutine != null) monoBehaviour.StopCoroutine(coroutine);
            coroutine = monoBehaviour.StartCoroutine(OnEnableAsync());
        }

        protected virtual IEnumerator OnEnableAsync()
        {
            yield break;
        }

        public virtual void OnDisable()
        {

        }

        public virtual void Enter()
        {
            state = State.enter;
            transitionTimer = 0;
            if(coroutine != null) monoBehaviour.StopCoroutine(coroutine);
            coroutine = monoBehaviour.StartCoroutine(EnterAsync());
        }

        protected virtual IEnumerator EnterAsync()
        {
            yield break;
        }

        public virtual void Update()
        {
            state = State.update;
            if(coroutine != null) monoBehaviour.StopCoroutine(coroutine);
            coroutine = monoBehaviour.StartCoroutine(UpdateAsync());
        }

        protected virtual IEnumerator UpdateAsync()
        {
            yield break;
        }

        public virtual void Exit()
        {
            state = State.exit;
            transitionTimer = 0;
            if(monoBehaviour.isActiveAndEnabled)
            {
                if(coroutine != null) monoBehaviour.StopCoroutine(coroutine);
                coroutine = monoBehaviour.StartCoroutine(ExitAsync());
            }
        }

        protected virtual IEnumerator ExitAsync()
        {
            yield break;
        }

        public virtual void PointerDown()
        {
            state = State.pointerDown;
            transitionTimer = 0;
            if(coroutine != null) monoBehaviour.StopCoroutine(coroutine);
            coroutine = monoBehaviour.StartCoroutine(PointerDownAsync());
        }

        protected virtual IEnumerator PointerDownAsync()
        {
            yield break;
        }

        public virtual void PointerUp()
        {
            state = State.pointerUp;
            transitionTimer = 0;
            if(coroutine != null) monoBehaviour.StopCoroutine(coroutine);
            coroutine = monoBehaviour.StartCoroutine(PointerUpAsync());
        }

        protected virtual IEnumerator PointerUpAsync()
        {
            yield break;
        }

        public enum State
        {
            onEnable,
            enter,
            update,
            exit,
            pointerDown,
            pointerUp
        }

        [Flags]
        public enum Type
        {
            none = 0,
            procedural = 1,
            color = 2,
            animation = 4,
            sprite = 8,
            text = 16,
            rendering = 32,
            audio = 64,
        }
    }
}
