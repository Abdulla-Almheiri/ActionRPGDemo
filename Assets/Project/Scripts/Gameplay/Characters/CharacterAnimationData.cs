using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterAnimationData
    {
        public CharacterAnimation Animation;
        public AnimationClip AnimationClip;
        public bool IsLooping = false;
        public float BaseAnimationSpeed = 1f;
        [Range(0f, 1f)]
        public float ImpactPoint = 0f;
        [Range(0f, 1f)]
        public float TrimAnimationFrom = 1f;
        [Range(0f, 5f)]
        public float TrimBlendDuration = 0.2f;
        public List<CharacterAttributeAnimationScalingData> AnimationScalingData = new List<CharacterAttributeAnimationScalingData>();
    }
}