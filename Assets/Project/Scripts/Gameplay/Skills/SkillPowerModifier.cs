using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    public class SkillPowerModifier
    {
        public float DamageIncrease = 0f;
        public float HealingIncrease = 0f;
        public float AdditionalDamageCriticalChance = 0f;
        public float AdditionalDamageCriticalAmount = 0f;
        public float AdditionalHealingCriticalChance = 0f;
        public float AdditionalHealingCriticalAmount = 0f;
        public float RechargeReduction = 0f;
        public float RechrageReductionFixed = 0f;
        public float EnergyCostReduction = 0f;
        public float EnergyCostReductionFixed = 0f;

        public static SkillPowerModifier operator + (SkillPowerModifier modifier1, SkillPowerModifier modifier2)
        {
            //Fix here. Implement the rest of the variables

            SkillPowerModifier newSkillModifier = new SkillPowerModifier();
            newSkillModifier.DamageIncrease = modifier1.DamageIncrease + modifier2.DamageIncrease;
            newSkillModifier.HealingIncrease = modifier1.HealingIncrease + modifier2.HealingIncrease;

            newSkillModifier.AdditionalDamageCriticalChance = modifier1.AdditionalDamageCriticalChance + modifier2.AdditionalDamageCriticalChance;
            return newSkillModifier;

        }
    }
}