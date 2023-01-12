using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;


namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterAttributeData 
    {
        public CharacterAttribute CharacterAttribute;
        public float BaseRating = 0f;

        public CharacterAttributeData(CharacterAttribute characterAttribute, float ratingValue)
        {
            CharacterAttribute = characterAttribute;
            BaseRating = ratingValue;
        }

       /* public float FinalValue(GameCombatProfile combatProfile, CharacterAttribute attribute = null)
        {
            if(attribute == null)
            {
                attribute = CharacterAttribute;
                if(attribute == null)
                {
                    return 0f;
                }
            }
            combatProfile.CharacterAttributeRatingConverstionDetails.Find(x => x.CharacterAttribute)
        }*/
    }
}
