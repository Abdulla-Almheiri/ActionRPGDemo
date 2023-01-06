using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterAttributeScalingData 
    {
        public CharacterAttribute CharacterAttribute;
        [Range(-10f, 10f)]
        [Tooltip("A scaling of 1 means a ratio of 1:1. Negative values mean inverse scaling.")]
        public float Scaling = 0f;

    }
}