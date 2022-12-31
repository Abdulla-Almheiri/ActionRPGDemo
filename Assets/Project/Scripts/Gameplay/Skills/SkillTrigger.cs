using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    public abstract class SkillTrigger : ScriptableObject
    {
        public abstract bool Evaluate(CharacterCombatController activator = null, CharacterCombatController receiver = null, SkillAction skillAction = null);
    }
}
