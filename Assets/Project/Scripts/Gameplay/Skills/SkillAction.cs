using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillAction 
    {
        public SkillTriggerData TriggerData;
        public CharacterAttribute ScalingAttribute;
        public float DamageScaled;
        public float DamageFlat;
        public float HealingScaled;
        public float HealingFlat;
        public float DrainHealthFlat;
        public float DrainHealthPercentageOfDamage;
        public float EnergyReturnFlat;
        public float EnergyReturnPercentageOfSkillEnergyCost;
        public float EnergyReturnPercentageOfDamageDone;
        public float EnergyReturnPercentageOfHealingDone;
        public CharacterState AppliesState;
    }
}