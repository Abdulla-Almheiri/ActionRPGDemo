using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public int ManaCost = 0;
        public float RechargeTime = 0f;
        
        /*
        [Space(20)]
        
        [Header("VISUAL EFFECT")]
       // public SkillPrefab DefaultVFXPrefab;
       // public SkillSpawnLocation PlayerSpawnLocation = SkillSpawnLocation.Center;
        [Space(20)]

        [Header("ACTIVATION")]
        [Range(0, 100)]
        public float TriggerChance = 100f;
       // public SkillTriggerCondition TriggerCondition;

        public bool FaceDirection = true;
        public bool IsMelee = false;
        public bool IsSpell = true;
        public bool IsMovementSkill = false;
        public bool RemovesRoot = false;
        public bool RemovesStun = false;
        public bool CanBeUsedWhileStunned = false;
        public bool RemovesSilence = false;

        public bool IsCastOnSelf = false;
       // public CharacterAnimationData PlayerAnimation;


        public Skill ImpactSkill;

        [Header("If Action's VFX Prefab is empty, then default one is used.")]
        public List<SkillAction> Actions;
        */

        public List<SkillAction_Direct> Direct = new List<SkillAction_Direct>();
        public List<SkillAction_CharacterEffect> Character = new List<SkillAction_CharacterEffect>();

        
    }
}