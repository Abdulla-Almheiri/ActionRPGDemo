using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Systems
{
    //TEST
    public class AnimatorPlaybackSystem
    {
        public delegate void AnimatorPlaybackSpeedChange(float newSpeed);
        private event AnimatorPlaybackSpeedChange OnAnimatorPlaybackSpeedChangedEvent;
        private float _baseAnimatorPlaybackSpeed = 1f;
        private float _currentAnimatorPlaybackSpeed = 1f;
        private float _baseAnimatorPlaybackSpeedModifier = 1f;
        private float _currentAnimatorPlaybackSpeedModifier = 1f;

        public AnimatorPlaybackSystem(AnimatorPlaybackSpeedChange subscriber = null, float baseAnimatorSpeed = 1f)
        {
            if (subscriber != null)
            {
                SubscribeToAnimatorPlaybackSpeedChange(subscriber);
            }

            _baseAnimatorPlaybackSpeed = baseAnimatorSpeed;
        }

        public void SubscribeToAnimatorPlaybackSpeedChange(AnimatorPlaybackSpeedChange subscriber)
        {
            OnAnimatorPlaybackSpeedChangedEvent += subscriber;
        }

        public void UnsubscribeToAnimatorPlaybackSpeedChange(AnimatorPlaybackSpeedChange unsubscriber)
        {
            OnAnimatorPlaybackSpeedChangedEvent -= unsubscriber;
        }

        private void OnAnimatorPlaybackSpeedChanged(float newSpeed)
        {
            OnAnimatorPlaybackSpeedChangedEvent?.Invoke(newSpeed);
        }
    }
}