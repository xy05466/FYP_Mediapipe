using System;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class TransitionPage : MonoBehaviour
    {
        protected Action OnAnimationFinished;

        // Events
        public void SubscribeTransitionEnded(Action action) => OnAnimationFinished += action;
        public void UnsubscribeTransitionEnded(Action action) => OnAnimationFinished -= action;
        public void ClearEvent() =>OnAnimationFinished = null;
    }
}

