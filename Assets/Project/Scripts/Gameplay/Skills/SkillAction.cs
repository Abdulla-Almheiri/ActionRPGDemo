using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    public abstract class SkillAction<T> : ScriptableObject where T: SkillActionData
    {
        public abstract void ApplyToCharacter(CharacterCombatController activator, CharacterCombatController receiver, SkillTemplate skillTemplate);
    }
}