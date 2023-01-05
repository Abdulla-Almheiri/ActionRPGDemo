using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public  class SkillActionCoefficient 
    {
        public float Value = 0f;
        public float Duration = 0f;
        public CharacterAttribute Attribute;


    }


}