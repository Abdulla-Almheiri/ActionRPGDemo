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
        public CharacterAttributeScalingData CharacterAttributeAnimationScalingData;

        public float GetFinalAnimationSpeed(CharacterCombatController combatController)
        {
            float multiplier = 1f;
            if(combatController == null)
            {
                return multiplier;
            }
            float additionalScaling = GetAnimationSpeedCharacterAttributeScalingMultiplierFromCombatController(combatController);
            multiplier += (CharacterAttributeAnimationScalingData.Scaling*additionalScaling);
            return multiplier*BaseAnimationSpeed;
        }

        public float GetFinalAnimationDuration(CharacterCombatController combatController)
        {
            var duration = GetBaseAnimationDuration() * GetFinalAnimationSpeed(combatController);
            return duration;
        }

        public float GetBaseAnimationDuration()
        {
            return AnimationClip.length;
        }

        public bool IsAnimationLooping()
        {
            return AnimationClip.isLooping;
        }
        private float GetAnimationSpeedCharacterAttributeScalingMultiplierFromCombatController(CharacterCombatController combatController)
        {
            float multiplier = 0f;
            if (combatController == null || CharacterAttributeAnimationScalingData.CharacterAttribute == null)
            {
                return multiplier;
            }
            float percentFromRating = combatController.GetPercentageFromRatingByCharacterAttribute(CharacterAttributeAnimationScalingData.CharacterAttribute);
            multiplier += (percentFromRating / 100f);
            return multiplier;
        }
    }
}