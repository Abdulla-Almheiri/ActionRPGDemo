using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [CreateAssetMenu(fileName = "new skill", menuName = "Content/Skills/Skill")]
    public class SkillTemplate : ScriptableObject
    {
        [Header("SKILL DESCRIPTION")]
        public Sprite Icon;
        public string Name;
        [TextArea(5, 15)]
        public string Description;

        [Space(20)]
        [Header("ACTIVATION")]
        public SkillActivationData SkillActivationData;
        public CharacterAction CharacterActionTriggered;

        [Space(20)]
        [Header("TRIGGER")]
        public SkillTriggerData SkillTriggerData;

        [Space(20)]
        [Header("VISUAL EFFECT")]
        public SkillEffectCombatController SkillPrefab;
        public SkillActionElement SkillElement;

        [Space(20)]
        [Header("Combat")]
        public List<SkillAction> SkillActions = new List<SkillAction>();

        
    }
}