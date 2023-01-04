using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character skill template", menuName = "Content/Characters/Character Skill Template")]
    public class CharacterSkillTemplate : ScriptableObject
    {
        public SkillTemplate PrimarySkill;
        public List<SkillTemplate> Skills = new List<SkillTemplate>();
    }
}