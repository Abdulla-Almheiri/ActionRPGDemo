using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.Characters
{

    public class CharacterCombatController : MonoBehaviour
    {
        [SerializeField]
        private CharacterCombatTemplate _characterCombatTemplate;
        private float _maxHealth = 100f;
        private float _currentHealth = 100f;
        private float _maxEnergy = 40f;
        private float _currentEnergy = 40f;
        private float _energyRegenPerSecond = 5f;

        private CharacterMovementController _characterMovementController;
        private CharacterMaterialController _characterMaterialController;
        private Dictionary<Attribute, CharacterAttributeValueContainer> _characterAttributes = new Dictionary<Attribute, CharacterAttributeValueContainer>();

        private CharacterStateTemplate _characterState;


        public void Start()
        {
            Initialize();
        }
        public void TakeDamage(float value)
        {
            //Debug.Log("Damage taken:   " + value);
        }

       public void ApplySkillAction(SkillAction skillAction, CharacterCombatController activator)
        {

        }
        
        private void ApplyHealingBySkillAction(SkillAction skillAction, CharacterCombatController activator)
        {

        }

        public void TriggerHitByElement(SkillActionElement skillElement)
        {
            Debug.Log("Method called");
            _characterMaterialController?.TriggerHitEffectBySkillActionElement(skillElement);
        }
        
        private void ClampHealth()
        {
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
        }

        private void ClampEnergy()
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0f, _maxEnergy);
        }

        public void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterMaterialController = GetComponent<CharacterMaterialController>();
        }

        public void EngageCharacterInMelee(CharacterCombatController targetCharacter)
        {
            if(_characterMovementController == null)
            {
                return;
            }


        }
    }
}