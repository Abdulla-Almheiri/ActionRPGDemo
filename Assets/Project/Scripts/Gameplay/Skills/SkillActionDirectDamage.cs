using Chaos.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    [CreateAssetMenu(fileName = "new directDamage skillAction", menuName = "Content/Skills/SkillAction DirectDamage")]
    public class SkillActionDirectDamage : SkillAction<SkillActionDataDirectDamage>
    {
        /*[SerializeField]
        protected SkillActionCoefficient _damage;
        [SerializeField]
        protected SkillActionCoefficient _healing;
        [SerializeField]
        protected SkillActionCoefficient _sacrificeDamage;
        [SerializeField]
        protected SkillActionCoefficient _friendlyFire;
        [SerializeField]
        protected SkillActionCoefficient _drainResource;*/
        public override void ApplyToCharacter(CharacterCombatController activator, CharacterCombatController receiver, SkillTemplate skillTemplate)
        {

        }
            
    }
}