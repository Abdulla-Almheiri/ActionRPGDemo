using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using UnityEngine.UI;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillActionData 
    {
        public SkillAction SkillAction;
        public SkillModifier SkillModifier;
        public SkillActionTypeEnum Type = SkillActionTypeEnum.Damage;
        public float Value = 0f;
        public float Duration = 0f;
        public CharacterAttribute Attribute;
        public SkillActionElement Element;
        public SkillVFXSpawnTemplate SkillVFXTemplate;
        
    }

    public enum SkillActionTypeEnum
    {
        Damage,
        Healing
    }
}