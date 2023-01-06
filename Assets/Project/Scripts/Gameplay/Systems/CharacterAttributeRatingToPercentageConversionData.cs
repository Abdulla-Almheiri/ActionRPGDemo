using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Systems
{
    [System.Serializable]
    public class CharacterAttributeRatingToPercentageConversionData
    {
        public CharacterAttribute CharacterAttribute;
        public float BaseRatingRequiredForOnePercentAtLevelOne = 10f;
        public float RatingRequiermentPercentIncreasePerAdditionalLevel = 20f;

        public float GetBasePercentageByRatingAndLevel(float rating, int level)
        {
            if(rating == 0f || level == 0)
            {
                return 0f;
            }

            if(BaseRatingRequiredForOnePercentAtLevelOne == 0f)
            {
                return 0f;
            }

            if(RatingRequiermentPercentIncreasePerAdditionalLevel <= 0f)
            {
                return BaseRatingRequiredForOnePercentAtLevelOne;
            }

            

            var result = rating / (BaseRatingRequiredForOnePercentAtLevelOne * Mathf.Pow(1f + (RatingRequiermentPercentIncreasePerAdditionalLevel / 100f), level-1));
            return result;
        }

        public float GetActualBasePercentageByRatingAndLevel(float rating, int level, CharacterCombatController combatController)
        {
            if(combatController == null)
            {
                return 0f;
            }
            var result = combatController.GetAttributeValue(CharacterAttribute);
            result = GetBasePercentageByRatingAndLevel(result, combatController.Level);
            return result;

        }
    }
}