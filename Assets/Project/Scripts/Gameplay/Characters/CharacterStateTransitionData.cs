using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterStateTransitionData 
    {
        public CharacterState State;
        public CharacterStateTransitionType Type = CharacterStateTransitionType.NormalizedBasedOnAnimation;
        [Range(0f, 10f)]
        public float Delay = 1f;
        public CharacterAttributeScalingData AttributeScaling;

        public float ActualDelayInSeconds(CharacterAnimationController animationController)
        {
            if (Type == CharacterStateTransitionType.FixedDelay)
            {
                return Delay;
            } else if(Type == CharacterStateTransitionType.NormalizedBasedOnAnimation)
            {
                var animationData = animationController.GetCharacterAnimationDataFromTemplate(State);
                if(animationData == null)
                {
                    return 0f;
                }
                var delay = animationController.GetActualAnimationDuration(animationData) * Delay;
                return delay;
            }

            return 0f;
        }

        private float AttributeScalingModifier(CharacterCombatController combatController)
        {
            var attributeValue = combatController.GetAttributeValue(AttributeScaling.CharacterAttribute);
            return attributeValue;
        }
    }

    public enum CharacterStateTransitionType
    {
        FixedDelay,
        NormalizedBasedOnAnimation,
    }
}