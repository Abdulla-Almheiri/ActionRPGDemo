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
        private List<SkillTemplate> _skills;
        private List<SkillEffectCombatController> _preloadedSkillPrefabs = new List<SkillEffectCombatController>();

        private float[] _rechargeDurations;
        void Start()
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
            _skills = CharacterSkillTemplate.Skills;
            PreloadSkillPrefabs();

            for (int i =0; i<_rechargeDurations.Length; i++)
            {
                _rechargeDurations[i] = 0f;
            }
            
        }

        private void PreloadSkillPrefabs()
        {
            foreach (SkillTemplate skill in _skills)
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


            skillCombatController.Initialize(skillTemplate, activator);
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
                Debug.Log("Template is null.");
                return false;
            }

            if(index >= CharacterSkillTemplate.Skills.Count)
            {
                Debug.Log("Skill index out of range.");
                return false;
            }

            if(IsSkillRecharging(index) == true)
            {
                Debug.Log("Skill is recharging.");
                return false;
            }

            TriggerSkillRecharge(index);
            SpawnSkillVFXTest(CharacterSkillTemplate.Skills[index]);
            _characterAnimationController.TriggerAnimation(CharacterSkillTemplate.Skills[index].CharacterAnimation);
            Debug.Log("Skill activated:  " + _skills[index].name);
            return true;
        }
        
        private void TriggerSkillRecharge(int index)
        {
            if(index >= _skills.Count)
            {
                return;
            }

            _rechargeDurations[index] = _skills[index].SkillActivationData.RechargeTime;
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
            if (index >= _skills.Count)
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
            var index = _skills.FindIndex(x => x == skill);
            return IsSkillRecharging(index);
        }

        private void ProcessSkillsRecharge()
        {
            for (int i = 0; i < _skills.Count; i++)
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