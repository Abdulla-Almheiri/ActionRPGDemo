using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Systems;
using System.Linq;

namespace Chaos.Gameplay.Characters
{

    public class CharacterCombatController : MonoBehaviour
    {
        public CharacterAttribute TESTATTRIBUTE;
        [field:SerializeField]
        public GameCombatProfile GameCombatProfile {private set; get;}
        public int Level { private set; get; } = 1;
        public GameUIController GameUIController;
        public CharacterAttribute TestIntellectAttribute;
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

        private Dictionary<CharacterAttribute, CharacterAttributeData> _characterAttributes = new Dictionary<CharacterAttribute, CharacterAttributeData>();

        private CharacterStateTemplate _characterState;


        public void Start()
        {
            //TEST
           if (TestIntellectAttribute != null)
            {
                _characterAttributes[TestIntellectAttribute] =  new CharacterAttributeData(TestIntellectAttribute, 50f);
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

            ResetAttributesFromCombatTemplate();

            SubscribeToCharacterStateChangedEvent();
        }
        public float GetAttributeValue(CharacterAttribute characterAttribute)
        {
            if(characterAttribute == null)
            {
                return 0f;
            }

            if(_characterAttributes.TryGetValue(characterAttribute, out CharacterAttributeData valueContainer))
            {
                //Fix here
                //Debug.Log("Attribute " + characterAttribute.name + "is   :   " + valueContainer.BaseRating);
                return valueContainer.BaseRating;
            }
            return 0f;
        }
        private void TakeDamage(float value)
        {
            _currentHealth -= value;
            ClampHealth();
            _characterUIController?.UpdateHealth();
            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform);
        }

        private void TakeHealing(float value)
        {
            _currentHealth += value;
            ClampHealth();
            _characterUIController?.UpdateHealth();
            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform);
        }

        public float GetPercentageFromRatingByCharacterAttribute(CharacterAttribute characterAttribute)
        {
            if(characterAttribute == null)
            {
                return 0f;
            }
            var attributeValue = GetAttributeValue(characterAttribute);
            return ConvertCharacterAttributeRatingToPercentageByGameCombatProfile(characterAttribute, attributeValue, GameCombatProfile);
        }

        private float ConvertCharacterAttributeRatingToPercentageByGameCombatProfile(CharacterAttribute characterAttribute, float rating, GameCombatProfile gameCombatProfile)
        {
            var attributeData = gameCombatProfile.CharacterAttributeRatingConverstionDetails.Find(x => x.CharacterAttribute == characterAttribute);
            if (attributeData == null)
            {
                return 0f;
            } else
            {
                return attributeData.GetActualBasePercentageByRatingAndLevel(rating, Level, this);
            }
        }
       public void ApplySkillAction(SkillActionData skillAction, CharacterCombatController activator)
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
        
        private void ApplyDamageBySkillAction(SkillActionData skillAction, CharacterCombatController activator)
        {
            if(activator == null)
            {
                return;
            }
            var finalValue = activator.GetAttributeValue(skillAction.Attribute) * skillAction.Value / 100f;
            TakeDamage(finalValue);
        }
        private void ApplyHealingBySkillAction(SkillActionData skillAction, CharacterCombatController activator)
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
                //TEST
               /* if(skillEffect.SkillTemplate.SkillActions.Find(x => x.Type == SkillActionTypeEnum.Damage) != null)
                {
                    return;
                }*/
            }
            TriggerHitByElement(skillEffect.SkillTemplate.SkillElement);
            _characterMaterialController?.TriggerHitFrame();
            _characterAnimationController?.TriggerHitAnimation();
        }

        public float GetHealthPercentage()
        {
            return _currentHealth / _maxHealth * 100f;
        }

        private void SubscribeToCharacterStateChangedEvent()
        {
            _characterStateController?.SubscribeToCharacterStateChanged(OnCharacterStateChanged);
        }
        private void ResetAttributesFromCombatTemplate()
        {
            if(CharacterCombatTemplate == null)
            {
                return;
            }

            foreach(CharacterAttributeData data in CharacterCombatTemplate.BaseCharacterAttributes)
            {
                _characterAttributes[data.CharacterAttribute] = data;

               if( _characterAttributes.TryGetValue(data.CharacterAttribute, out CharacterAttributeData characterAttributeData))
                {
                }
            }
        }

        private void OnCharacterStateChanged(CharacterState newCharacterState, float delay = 0f)
        {
            //Debug.Log("Character state changed to  :  CanBAsicAttack  :  " + newCharacterState.CanBasicAttack);
        }
    }
}