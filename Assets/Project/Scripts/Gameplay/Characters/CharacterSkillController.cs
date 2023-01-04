using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterSkillController : MonoBehaviour
    {
        public GameUIController GameUIController;
        public GameSkillController GameSkillController;
        public SkillTemplate SkillTemplateTest;
        public GameObject SkillVFX;
        public CharacterVFXSpawnLocationType SpawnLocationType;

        protected CharacterVFXController _characterVFXController;
        protected CharacterMovementController _characterMovementController;
        private CharacterAnimationController _characterAnimationController;
        private CharacterCombatController _characterCombatController;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void Initialize()
        {
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            _characterVFXController = GetComponent<CharacterVFXController>();
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
        }
        public void SpawnSkillVFXTest()
        {
            if(SkillVFX == null || _characterVFXController == null || SpawnLocationType == null)
            {
                return;
            }
            _characterMovementController.RotateCharacterInMouseDirection();
            _characterMovementController.StopMovement();
            var spawnPoint = _characterVFXController.GetVFXSpawnTransform(SpawnLocationType);

            if(spawnPoint == null)
            {
                return;
            }
            GameSkillController.SpawnSkillEffectPooled(SkillTemplateTest, spawnPoint, _characterCombatController);
            
        }


        public void ActivateSkill(int number)
        {
            if(number == 1)
            {
                SpawnSkillVFXTest();
                _characterAnimationController.TriggerAttackAnimation();
            }
        }
        
    }
}