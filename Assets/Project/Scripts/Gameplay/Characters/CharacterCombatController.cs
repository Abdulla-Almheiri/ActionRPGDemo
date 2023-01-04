using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{

    public class CharacterCombatController : MonoBehaviour
    {
        public GameUIController GameUIController;
        public Attribute TestIntellectAttribute;
        [field:SerializeField]
        public CharacterCombatTemplate CharacterCombatTemplate { private set; get; }
        private float _maxHealth = 100f;
        private float _currentHealth = 100f;
        private float _maxEnergy = 40f;
        private float _currentEnergy = 40f;
        private float _energyRegenPerSecond = 5f;

        private CharacterMovementController _characterMovementController;
        private CharacterMaterialController _characterMaterialController;
        private CharacterAnimationController _characterAnimationController;
        private CharacterUIController _characterUIController;
        private CharacterStateController _characterStateController;

        private Dictionary<Attribute, CharacterAttributeValueContainer> _characterAttributes = new Dictionary<Attribute, CharacterAttributeValueContainer>();

        private CharacterStateTemplate _characterState;


        public void Start()
        {
            //TEST
            if (TestIntellectAttribute != null)
            {
                _characterAttributes.Add(TestIntellectAttribute, new CharacterAttributeValueContainer(50f, 0f));
            }

            Initialize();
        }
        public void Initialize()
        {
            _characterUIController = GetComponent<CharacterUIController>();
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterMaterialController = GetComponent<CharacterMaterialController>();
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            _characterStateController = GetComponent<CharacterStateController>();

            SubscribeToCharacterStateChangedEvent();
        }
        public float GetAttributeValue(Attribute attribute)
        {
            if(_characterAttributes.TryGetValue(attribute, out CharacterAttributeValueContainer valueContainer))
            {
                return valueContainer.Value;
            }
            return 0f;
        }
        private void TakeDamage(float value)
        {
            _currentHealth -= value;
            ClampHealth();
            _characterUIController?.UpdateHealth();
            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform);
            Debug.Log("Damage taken:   " + value);
        }

        private void TakeHealing(float value)
        {
            _currentHealth += value;
            ClampHealth();
            _characterUIController?.UpdateHealth();
            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform);
            Debug.Log("Healing taken:   " + value);
        }

       public void ApplySkillAction(SkillAction skillAction, CharacterCombatController activator)
        {
            if(skillAction.Type == SkillActionTypeEnum.Damage && activator != this)
            {
                ApplyDamageBySkillAction(skillAction, activator);
            }

            if (skillAction.Type == SkillActionTypeEnum.Healing && activator == this)
            {
                ApplyHealingBySkillAction(skillAction, activator);
            }
        }
        
        private void ApplyDamageBySkillAction(SkillAction skillAction, CharacterCombatController activator)
        {
            if(activator == null)
            {
                return;
            }
            var finalValue = activator.GetAttributeValue(skillAction.Attribute) * skillAction.Value / 100f;
            TakeDamage(finalValue);
        }
        private void ApplyHealingBySkillAction(SkillAction skillAction, CharacterCombatController activator)
        {
            if (activator == null)
            {
                return;
            }
            var finalValue = activator.GetAttributeValue(skillAction.Attribute) * skillAction.Value / 100f;
            TakeHealing(finalValue);
        }

        public void TriggerHitByElement(SkillActionElement skillElement)
        {
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



        public void EngageCharacterInMelee(CharacterCombatController targetCharacter)
        {
            if(_characterMovementController == null)
            {
                return;
            }


        }

        public void TriggerHitBySkillEffect(SkillEffectCombatController skillEffect, CharacterCombatController activator)
        {
            bool isActivatedBySelf = activator == this;
            if(isActivatedBySelf == true)
            {
                if(skillEffect.SkillTemplate.SkillActions.Find(x => x.Type == SkillActionTypeEnum.Damage) != null)
                {
                    return;
                }
            }
            TriggerHitByElement(skillEffect.SkillTemplate.SkillElement);
            _characterMaterialController?.TriggerHitFrame();
            _characterAnimationController?.TriggerHitAnimation();
        }

        public float GetHealthPercentage()
        {
            Debug.Log("Health Percentage is     :   " + _currentHealth / _maxHealth * 100f);
            return _currentHealth / _maxHealth * 100f;
        }

        private void SubscribeToCharacterStateChangedEvent()
        {
            _characterStateController?.SubscribeToCharacterStateChanged(OnCharacterStateChanged);
        }


        private void OnCharacterStateChanged(CharacterState newCharacterState, float delay = 0f)
        {
            //Debug.Log("Character state changed to  :  CanBAsicAttack  :  " + newCharacterState.CanBasicAttack);
        }
    }
}