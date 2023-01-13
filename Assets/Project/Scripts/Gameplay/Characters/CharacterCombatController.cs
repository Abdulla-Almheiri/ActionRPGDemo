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
        private Rigidbody _characterRigidBody;

        private Dictionary<CharacterAttribute, CharacterAttributeData> _characterAttributes = new Dictionary<CharacterAttribute, CharacterAttributeData>();
        public Dictionary<CharacterAttribute, float> CoreCharacterAttributes { private set; get; } = new Dictionary<CharacterAttribute, float>();

        public bool Alive { set; get; } = true;
        private CharacterStateTemplate _characterState;


        public void Start()
        {
            //TEST
           if (TestIntellectAttribute != null)
            {
                _characterAttributes[TestIntellectAttribute] =  new CharacterAttributeData(TestIntellectAttribute, 50f);
                CoreCharacterAttributes[TestIntellectAttribute] = 70f;
            }

            Initialize();
        }
        public void Update()
        {
            ProcessEnergyRegen();
        }

        private void ProcessEnergyRegen()
        {
            if(_currentEnergy >= _maxEnergy)
            {
                return;
            }

            _currentEnergy += (_energyRegenPerSecond / Time.deltaTime);
            ClampEnergy();
        }
        public void Initialize()
        {
            _characterUIController = GetComponent<CharacterUIController>();
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterMaterialController = GetComponent<CharacterMaterialController>();
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            _characterStateController = GetComponent<CharacterStateController>();
            _characterRigidBody = GetComponent<Rigidbody>();

           // ResetAttributesFromCombatTemplate();

            SubscribeToCharacterStateChangedEvent();
            Alive = true;
            _currentHealth = _maxHealth;
            _currentEnergy = _maxEnergy;
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

        public void TriggerHitBySkillAction(CharacterCombatController activator, SkillTemplate skill, SkillAction skillAction, List<SkillModifier> modifiers = null)
        {
            if (Alive == false)
            {
                return;
            }


            bool hasAttribute = activator.CoreCharacterAttributes.TryGetValue(skillAction.ScalingAttribute, out float attributeValue);
            float energyReturn = 0f;
            Debug.Log("Has attribute  :   " + hasAttribute);
            if(hasAttribute && attributeValue != 0f)
            {
                if (skillAction.DamageScaled != 0)
                {
                    var damage = (skillAction.DamageScaled / 100f) * attributeValue;
                    TakeDamage(damage);

                    if(skillAction.DrainHealthPercentageOfDamage != 0)
                    {
                        var drain = (skillAction.DrainHealthPercentageOfDamage / 100f) * damage;
                        TakeHealing(drain);
                    }

                    if (skillAction.EnergyReturnPercentageOfDamageDone != 0)
                    {
                        energyReturn = (skillAction.EnergyReturnPercentageOfDamageDone / 100f) * damage;
                        
                    }
                }

                if (skillAction.HealingScaled != 0)
                {
                    var healing = (skillAction.HealingScaled / 100f) * attributeValue;
                    TakeHealing(healing);

                    if (skillAction.EnergyReturnPercentageOfHealingDone != 0)
                    {
                        energyReturn = (skillAction.EnergyReturnPercentageOfHealingDone / 100f) * healing;
                    }
                }
            }


            if(energyReturn != 0)
            {
                GenerateEnergy(energyReturn);
            }


 



        }

        private void GenerateEnergy(float amount)
        {
            if (Alive == false)
            {
                return;
            }

            _currentEnergy += amount;
            ClampEnergy();
        }

        private void TakeDamage(float value)
        {
            if (Alive == false)
            {
                return;
            }

            _currentHealth -= value;
            ClampHealth();
            _characterUIController?.UpdateHealth();
            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform);

            if(_currentHealth <= 0f)
            {
                _characterStateController.TriggerDeathState();
            }
        }

        private void TakeHealing(float value)
        {
            if (Alive == false)
            {
                return;
            }

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
            if (Alive == false)
            {
                return;
            }


            if (skillAction.Type == SkillActionTypeEnum.Damage && activator != this)
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
            if (Alive == false)
            {
                return;
            }


            if (activator == null)
            {
                return;
            }
            var finalValue = activator.GetAttributeValue(skillAction.Attribute) * skillAction.Value / 100f;
            TakeDamage(finalValue);
        }
        private void ApplyHealingBySkillAction(SkillActionData skillAction, CharacterCombatController activator)
        {
            if (Alive == false)
            {
                return;
            }

            if (activator == null)
            {
                return;
            }
            var finalValue = activator.GetAttributeValue(skillAction.Attribute) * skillAction.Value / 100f;
            TakeHealing(finalValue);
        }

        public void TriggerHitByElement(SkillActionElement skillElement)
        {
            if (Alive == false)
            {
                return;
            }

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
            if (Alive == false)
            {
                return;
            }

            if (_characterMovementController == null)
            {
                return;
            }


        }

        public void TriggerHitBySkillEffect(SkillEffectCombatController skillEffect, CharacterCombatController activator)
        {
            if(Alive == false)
            {
                return;
            }

            bool isActivatedBySelf = activator == this;
            if(isActivatedBySelf == true)
            {
                //TEST
               /* if(skillEffect.SkillTemplate.SkillActions.Find(x => x.Type == SkillActionTypeEnum.Damage) != null)
                {
                    return;
                }*/
            }
            //_characterUIController.GameUIController.SpawnDamageTextAtScreenPositionTest(Random.Range(0, 150).ToString(), transform);
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

            foreach(CharacterAttributeData data in CharacterCombatTemplate.PrimaryCharacterAttributes)
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

        public void TriggerDeath()
        {
            Alive = false;
            _currentHealth = 0f;
            _currentEnergy = 0f;
            _characterRigidBody.detectCollisions = false;
        }

        public void Revive()
        {
            _characterUIController.UpdateHealth();
            Alive = true;
            _currentHealth = _maxHealth;
            _currentEnergy = _maxEnergy;
            _characterRigidBody.detectCollisions = true;
        }
    }
}