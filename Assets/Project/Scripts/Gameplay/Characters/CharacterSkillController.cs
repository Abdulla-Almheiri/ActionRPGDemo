using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.Player;

namespace Chaos.Gameplay.Characters
{
    public class CharacterSkillController : MonoBehaviour
    {
        [field:SerializeField]
        public CharacterSkillTemplate CharacterSkillTemplate { get; private set; }
        public GameUIController GameUIController;
        public GameSkillController GameSkillController;
        public SkillTemplate SkillTemplateTest;
        public SkillTemplate SkillTemplateHolyLight;

        public GameObject SkillVFX;
        public CharacterVFXSpawnLocationType SpawnLocationType;

        private CharacterVFXController _characterVFXController;
        private CharacterMovementController _characterMovementController;
        private CharacterAnimationController _characterAnimationController;
        private CharacterCombatController _characterCombatController;
        private PlayerController _playerController;
        private CharacterInputController _characterInputController;
        public List<SkillTemplate> Skills { private set; get; }
        private List<SkillEffectCombatController> _preloadedSkillPrefabs = new List<SkillEffectCombatController>();
        public SkillEffectCombatController CurrentActiveSkillEffect { private set; get; }
        private float[] _rechargeDurations;
        void Awake()
        {
            Initialize();
        }

        void Update()
        {
            ProcessSkillsRecharge();
        }

        protected void Initialize()
        {
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            _characterVFXController = GetComponent<CharacterVFXController>();
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
            _characterInputController = GetComponent<CharacterInputController>();
            _characterInputController = GetComponent<CharacterInputController>();
            _playerController = GetComponent<PlayerController>();

            _rechargeDurations = new float[7];
            Skills = CharacterSkillTemplate.Skills;
            PreloadSkillPrefabs();

            for (int i =0; i<_rechargeDurations.Length; i++)
            {
                _rechargeDurations[i] = 0f;
            }
            
        }

        private void PreloadSkillPrefabs()
        {
            foreach (SkillTemplate skill in Skills)
            {
                var instance = Instantiate(skill.SkillPrefab, gameObject.transform);
                if (instance != null)
                {
                    instance.gameObject.transform.parent = null;
                    _preloadedSkillPrefabs.Add(instance);
                    instance.gameObject.SetActive(false);
                }
            }
        }
        public void SpawnSkillVFXTest(SkillTemplate skillTemplate)
        {
            /*if(SkillVFX == null || _characterVFXController == null || SpawnLocationType == null)
            {
                return;
            }*/

            if(_playerController != null)
            {
                _characterMovementController.RotateCharacterInMouseDirection();
            }

            //_characterMovementController.StopMovementAndRequestStateChangeToIdle();
            var spawnPoint = _characterVFXController.GetVFXSpawnTransform(skillTemplate.SpawnLocationType);

            if(spawnPoint == null)
            {
                return;
            }
            SpawnSkillVFX(skillTemplate, spawnPoint, _characterCombatController);
            
        }

        
        public bool SpawnSkillVFX(SkillTemplate skillTemplate, Transform location, CharacterCombatController activator)
        {
            if (skillTemplate == null || skillTemplate.SkillPrefab == null)
            {
                return false;
            }

            var instance = InstantiateSkillEffect(skillTemplate, location, activator);
            CurrentActiveSkillEffect = instance;
            return true;
        }

        private SkillEffectCombatController InstantiateSkillEffect(SkillTemplate skillTemplate, Transform location, CharacterCombatController activator)
        {
            var spawn = Instantiate(skillTemplate.SkillPrefab, location);
            spawn.transform.SetParent(null);

            var skillCombatController = spawn.GetComponent<SkillEffectCombatController>();

            if (skillCombatController == null)
            {
                return null;
            }

            //FIX HERE
            skillCombatController.Initialize(skillTemplate, activator, 0.3f, skillTemplate.IsSelfSkill);
            return skillCombatController;
        }

        public bool ActivateSkill(int index)
        {
            if (_characterCombatController.Alive == false)
            {
                return false;
            }

            if(CharacterSkillTemplate == null)
            {
                //Debug.Log("Template is null.");
                return false;
            }

            if(index >= CharacterSkillTemplate.Skills.Count)
            {
               // Debug.Log("Skill index out of range.");
                return false;
            }

            if(IsSkillRecharging(index) == true)
            {
                //Debug.Log("Skill is recharging.");
                return false;
            }

            if(_characterCombatController.IsEnergyEnoughForSkill(Skills[index]) == false)
            {
                return false;
            }

            _characterCombatController.ConsumeEnergyFromSkill(Skills[index]);
            TriggerSkillRecharge(index);
            SpawnSkillVFXTest(CharacterSkillTemplate.Skills[index]);
            _characterAnimationController.TriggerAnimation(CharacterSkillTemplate.Skills[index].CharacterAnimation);
            //Debug.Log("Skill activated:  " + _skills[index].name);
            return true;
        }
        
        private void TriggerSkillRecharge(int index)
        {
            if(index >= Skills.Count)
            {
                return;
            }

            _rechargeDurations[index] = Skills[index].SkillActivationData.RechargeTime;
        }
        public bool IsSkillRecharging(int index)
        {
            if(GetRemainingSkillRechargeTime(index) > 0f)
            {
                return true;
            }

            return false;
        }

        public float GetRemainingSkillRechargeTime(int index)
        {
            if (index >= Skills.Count)
            {
                return -1f;
            }

            if (_rechargeDurations[index] > 0f)
            {
                return _rechargeDurations[index];
            }

            return 0f;
        }


        public bool IsSkillRecharging(SkillTemplate skill)
        {
            var index = Skills.FindIndex(x => x == skill);
            return IsSkillRecharging(index);
        }

        private void ProcessSkillsRecharge()
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                if (_rechargeDurations[i] > 0f)
                {
                    _rechargeDurations[i] -= Time.deltaTime;
                }
                else
                {
                    _rechargeDurations[i] = 0f;
                }
            }
        }
    }
}