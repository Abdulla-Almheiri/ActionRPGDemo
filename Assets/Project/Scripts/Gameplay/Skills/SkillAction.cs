using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using UnityEngine.UI;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public  class SkillAction 
    {
        public SkillActionCoefficient Damage;
        public SkillActionCoefficient Healing;
        public SkillActionElement Element;
        public float Duration = 10f;
        
    }
}