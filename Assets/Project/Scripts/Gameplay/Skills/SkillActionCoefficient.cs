using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public  class SkillActionCoefficient 
    {
        public float Value;
        public CooefficientType CooefficientType;
        public Attribute Attribute;
    }

    public enum CooefficientType
    {
        PERCENTAGE,
        FLAT
    }
}