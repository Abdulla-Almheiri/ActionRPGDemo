using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillModifier
    {
        public CharacterAttribute Attribute;
        [Range(-10f, 10f)]
        [Tooltip("A coefficient of 1 means 100% of Attribute value will be used.")]
        public float Coefficient = 1f;
        private float _cachedResults = 0f;
        public float Result(CharacterCombatController character)
        {
            if(Attribute == null)
            {
                return 0f;
            }
            _cachedResults = character.GetAttributeValue(Attribute);
            return _cachedResults;
        }

        public float CachedResult()
        {
            return _cachedResults;
        }

    }
}