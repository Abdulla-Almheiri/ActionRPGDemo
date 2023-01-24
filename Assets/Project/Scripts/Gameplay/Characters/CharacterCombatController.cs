using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Systems;
using System.Linq;
using Chaos.Gameplay.Player;

namespace Chaos.Gameplay.Characters
{

    public class CharacterCombatController : MonoBehaviour
    {
        public SkillTemplate SkillTemplateModifierTest;
        public bool IsPlayer { private set; get; } = false;
        public CharacterAttribute TESTATTRIBUTE;
        [field:SerializeField]
        public GameCombatProfile GameCombatProfile {private set; get;}
        public int Level { private set; get; } = 1;
        [field: SerializeField]
        public CharacterFaction CharacterFaction { private set; get; }
        public GameUIController GameUIController;
        public CharacterAttribute TestIntellectAttribute;
        [field:SerializeField]
        public CharacterCombatTemplate CharacterCombatTemplate { private set; get; }
        private float _maxHealth = 100f;
        private float _currentHealth = 100f;
        private float _maxEnergy = 40f;
        private float _currentEnergy = 40f;
        private float _energyRegenPerSecond = 5f;
        private float _criticalDamageChance = 10f;

        private float _criticalDamageMultiplier = 2f;
        private float _criticalHealChance = 50f;
        private float _criticalHealMultiplier = 2f;


        private CharacterMovementController _characterMovementController;
        private CharacterMaterialController _characterMaterialController;
        private CharacterAnimationController _characterAnimationController;
        private CharacterUIController _characterUIController;
        private CharacterStateController _characterStateController;
        private CharacterAIController _characterAIController;
        private Rigidbody _characterRigidBody;

        private Dictionary<CharacterAttribute, CharacterAttributeData> _characterAttributes = new Dictionary<CharacterAttribute, CharacterAttributeData>();
        public Dictionary<CharacterAttribute, float> CoreCharacterAttributes { private set; get; } = new Dictionary<CharacterAttribute, float>();
        public Dictionary<SkillTemplate, SkillPowerModifier> SkillPowerModifiers { private set; get; } = new Dictionary<SkillTemplate, SkillPowerModifier>();
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

            //TEST CODE. Remove later
            if(SkillTemplateModifierTest != null)
            {
                SkillPowerModifier skillModifier = new SkillPowerModifier();
                skillModifier.DamageIncrease = 30f;
                skillModifier.AdditionalDamageCriticalChance = 50f;
                skillModifier.AdditionalDamageCriticalAmount = 50f;

                AddSkillPowerModifier(SkillTemplateModifierTest, skillModifier);
                Debug.Log("Skillmodifiers.Count ==   " + SkillPowerModifiers.Count);
            }

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
            _characterAIController = GetComponent<CharacterAIController>(); 

            _characterRigidBody = GetComponentInChildren<Rigidbody>();

            if(GetComponent<PlayerController>() !=  null)
            {
                IsPlayer = true;
            }

           UpdateCharacterAttributesFromCombatTemplate();

            SubscribeToCharacterStateChangedEvent();
            Alive = true;
            _currentHealth = _maxHealth;
            _currentEnergy = _maxEnergy;
        }

        public void AddSkillPowerModifier(SkillTemplate skill, SkillPowerModifier skillPowerModifier)
        {
            if(SkillPowerModifiers.TryAdd(skill, skillPowerModifier) == false)
            {
                SkillPowerModifiers[skill] = SkillPowerModifiers[skill] + skillPowerModifier;
            } else
            {
                SkillPowerModifiers[skill] = skillPowerModifier;
            }
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

        //Ugly method. FIX!!!!
        public void TriggerHitBySkillAction(CharacterCombatController activator, SkillTemplate skill, SkillAction skillAction, List<SkillModifier> modifiers = null)
        {
            if (Alive == false)
            {
                return;
            }
            bool hasSkillModifier = activator.SkillPowerModifiers.TryGetValue(skill, out SkillPowerModifier skillModifier);

            bool hasAttribute = activator.CoreCharacterAttributes.TryGetValue(skillAction.ScalingAttribute, out float attributeValue);
            float energyReturn = 0f;

            if(hasAttribute && attributeValue != 0f)
            {
                if (skillAction.DamageScaled != 0 && activator != this)
                {
                    var damage = (skillAction.DamageScaled / 100f) * attributeValue;
                    var criticalChance = _criticalDamageChance;
                    var criticalMultiplier = _criticalDamageMultiplier;

                    if (hasSkillModifier)
                    {
                        damage += (damage * skillModifier.DamageIncrease/100f);
                        criticalChance += skillModifier.AdditionalDamageCriticalChance;
                        criticalMultiplier += (skillModifier.AdditionalDamageCriticalAmount / 100f);
                    }
                    if(Random.Range(0f, 100f) < criticalChance)
                    {

                        damage *= criticalMultiplier;
                        TakeDamage(damage, true);
                    } else
                    {
                        TakeDamage(damage, false);
                    }
                    
                    //FIX HERE. Apply skillmodifier to healing.
                    if(skillAction.DrainHealthPercentageOfDamage != 0)
                    {
                        var drain = (skillAction.DrainHealthPercentageOfDamage / 100f) * damage;
                        TakeHealing(drain, false);
                    }

                    if (skillAction.EnergyReturnPercentageOfDamageDone != 0)
                    {
                        energyReturn = (skillAction.EnergyReturnPercentageOfDamageDone / 100f) * damage;
                        
                    }
                }

                if (skillAction.HealingScaled != 0 && activator == this)
                {
                    Debug.Log("HEALING TIME!!!!!!!!!!!!!");
                    var healing = (skillAction.HealingScaled / 100f) * attributeValue;

                    if (Random.Range(0f, 100f) < _criticalHealChance)
                    {
                        healing *= _criticalHealMultiplier;
                        TakeHealing(healing, true);
                    }
                    else
                    {
                        TakeHealing(healing, false);
                    }

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

        private void TakeDamage(float value, bool isCritical)
        {
            if (Alive == false)
            {
                return;
            }

            _currentHealth -= value;
            //Fix here. Magic number!
            SetAIAlertnessLevel(5f, 3f);
            ClampHealth();
            _characterUIController?.UpdateHealth();
            FloatingCombatTextEventType combatEventType = FloatingCombatTextEventType.Damage;
            if(isCritical)
            {
                combatEventType = FloatingCombatTextEventType.CriticalDamage;
            }
            if(IsPlayer == true)
            {
                combatEventType = FloatingCombatTextEventType.DamageTaken;
            }

            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform, combatEventType);

            if(_currentHealth <= 0f)
            {
                _characterStateController.TriggerDeathState();
            }
        }

        private void SetAIAlertnessLevel(float amount, float duration)
        {
            if(_characterAIController == null)
            {
                return;
            }

            _characterAIController.SetAlertnessLevel(amount, duration);

        }
        private void TakeHealing(float value, bool isCritical)
        {
            if (Alive == false)
            {
                return;
            }

            _currentHealth += value;
            ClampHealth();
            _characterUIController?.UpdateHealth();
            FloatingCombatTextEventType combatEventType = FloatingCombatTextEventType.Healing;
            if (isCritical)
            {
                combatEventType = FloatingCombatTextEventType.CriticalHealing;
            }
            GameUIController?.SpawnDamageTextAtScreenPositionTest(Mathf.Floor(value).ToString(), transform, combatEventType);
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
            //TakeDamage(finalValue);
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
            //TakeHealing(finalValue);
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
                TriggerHitByElement(skillEffect.SkillTemplate.SkillElement);
            } else
            {

                _characterMaterialController?.TriggerHitFrame();
                _characterAnimationController?.TriggerHitAnimation();
            }
            //_characterUIController.GameUIController.SpawnDamageTextAtScreenPositionTest(Random.Range(0, 150).ToString(), transform);

        }

        public float GetHealthPercentage()
        {
            return _currentHealth / _maxHealth * 100f;
        }

        private void SubscribeToCharacterStateChangedEvent()
        {
            _characterStateController?.SubscribeToCharacterStateChanged(OnCharacterStateChanged);
        }
        private void UpdateCharacterAttributesFromCombatTemplate()
        {
            if(CharacterCombatTemplate == null)
            {
                return;
            }

            foreach(CharacterAttributeData data in CharacterCombatTemplate.PrimaryCharacterAttributes)
            {
                if(CoreCharacterAttributes.TryGetValue(data.CharacterAttribute, out float value) == true)
                {
                    CoreCharacterAttributes[data.CharacterAttribute] = data.BaseRating;
                } else
                {
                    CoreCharacterAttributes.Add(data.CharacterAttribute, data.BaseRating);
                }
            }

            _currentHealth = CharacterCombatTemplate.Health.BaseRating;
            _maxHealth = _currentHealth;
            _currentEnergy = CharacterCombatTemplate.Energy.BaseRating;
            _maxEnergy = _currentEnergy;
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
            _characterMovementController.DisableNavMeshComponent();
            Debug.Log("Death triggered");
        }

        public void Revive()
        {
            _characterUIController.UpdateHealth();
            Alive = true;
            _currentHealth = _maxHealth;
            _currentEnergy = _maxEnergy;
            _characterRigidBody.detectCollisions = true;

            _characterMovementController.EnableNavMeshComponent();
        }
    }
}