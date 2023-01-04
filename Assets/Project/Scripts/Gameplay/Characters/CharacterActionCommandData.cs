using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterActionCommandData
    {
        public float PercentageOfAnimationDuration = 0.5f;
        public float MinDuration = 0.1f;
        public float MaxDuration = 2f;
    }
}