using Chaos.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    [CreateAssetMenu(fileName = "new skill element trigger", menuName = "Content/Skills/Skill Element Trigger")]
    public class SkillTriggerBySkillElement : SkillTrigger
    {
        public bool SkillElementReceived = false;
        public bool SkillElementDone = true;
        public override bool Evaluate(CharacterCombatController activator = null, CharacterCombatController receiver = null, SkillAction skillAction = null)
        {
            if(skillAction == null)
            {
                return false;
            }

            return skillAction.Element == SkillElementDone || skillAction.Element == SkillElementReceived;
        }
    }

    
}