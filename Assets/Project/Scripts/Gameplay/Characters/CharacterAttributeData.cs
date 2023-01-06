using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
