using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterAttributeValueContainer 
    {
        public float Value = 0f;
        public float Multiplier = 1f;

        public CharacterAttributeValueContainer(float value, float multiplier)
        {
            Value = value;
            Multiplier = multiplier;
        }
    }
}