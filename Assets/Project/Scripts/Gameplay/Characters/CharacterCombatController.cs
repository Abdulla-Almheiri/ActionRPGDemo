using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.Characters
{

    public class CharacterCombatController : MonoBehaviour
    {
        private float _maxHealth = 100f;
        private float _currentHealth = 100f;
        private float _maxEnergy = 40f;
        private float _currentEnergy = 40f;
        private float _energyRegenPerSecond = 5f;

        private CharacterMovementController _characterMovementController;

        private Dictionary<Attribute, float> _attributes = new Dictionary<Attribute, float>();

        private CharacterStateTemplate _characterState;
        public void TakeDamage(float value)
        {
            Debug.Log("Damage taken:   " + value);
        }

       public void ApplySkillAction(SkillAction skillAction)
        {

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