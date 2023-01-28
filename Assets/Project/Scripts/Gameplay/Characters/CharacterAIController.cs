using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAIController : MonoBehaviour
    {
        public GameObject PlayerTest;
        private CharacterMovementController _playerMovementController;
        private CharacterCombatController _playerCombatController;
        [SerializeField]
        protected CharacterAITemplate _characterAITemplate;
        private CharacterMovementController _characterMovementController;
        private CharacterSkillController _characterSkillController;
        private CharacterAudioController _characterAudioController;
        private CharacterCombatController _characterCombatController;

        private float _decisionRecharge = 1f;
        private float _decisionRechargeCounter = 0f;
        private float _baseAlertnessLevel = 1f;
        private float _alertnessLevel = 1f;
        private float _alertnessLevelCounter = 0f;
        private bool _engaged = false;
        private bool _engagedSoundPlayed = false;
        private float _engageSoundRecharge = 10f;
        private float _engageSoundCounter = 0f;
        void Start()
        {
            Initialize();
        }

        void Update()
        {
            ProcessAlertnessCounter();
            ProcessCharacterAI();
            ProcessAIDecisionRecharge();
            ProcessEngageSoundRecharge();
        }

        private void ProcessEngageSoundRecharge()
        {
            if(_engageSoundCounter > 0f)
            {
                _engageSoundCounter -= Time.deltaTime;
            }
        }

        private void PlayEngageSound()
        {
            if(_characterAudioController == null)
            {
                return;
            }

            _engageSoundCounter = _engageSoundRecharge;
            _characterAudioController.PlayEngageSound();
            
        }

        private bool IsEngageSoundReady()
        {
            return _engageSoundCounter <= 0f;
        }
        private void ResetAlertness()
        {
            _alertnessLevel = _baseAlertnessLevel;
        }

        public void SetAlertnessLevel(float value, float duration)
        {
            _alertnessLevel = Mathf.Max(0f, value);
            _alertnessLevelCounter = Mathf.Max(0f, duration);
        }

        private void ProcessAlertnessCounter()
        {
            if(_alertnessLevelCounter > 0f)
            {
                _alertnessLevelCounter -= Time.deltaTime;
            } else {
                ResetAlertness();
            }
        }

        public void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterSkillController = GetComponent<CharacterSkillController>();
            _playerCombatController = PlayerTest.GetComponent<CharacterCombatController>();
            _characterAudioController = GetComponent<CharacterAudioController>();
            _characterCombatController = GetComponent<CharacterCombatController>();

            if (PlayerTest != null)
            {
                _playerMovementController = PlayerTest.GetComponent<CharacterMovementController>();
            }
        }

        private void ProcessCharacterAI()
        {
            if(_characterAITemplate == null)
            {
                return;
            }

            if(_characterCombatController?.Alive == false )
            {
                return;
            }

            if(_characterMovementController.GetUnsignedDistanceBetweenCharacters(_playerMovementController) >= _characterAITemplate.PlayerDetectionRange*_alertnessLevel)
            {
                TriggerAIDecisionRecharge();
                _engaged = false;
                _engagedSoundPlayed = false;
                return;
            }

            _engaged = true;
            if(IsEngageSoundReady() == true)
            {
                PlayEngageSound();
            }

            ProcessChaseBehaviour();
            ProcessRangedAttackBehaviour();
        }

        private void ProcessRangedAttackBehaviour()
        {
            if (_playerCombatController.Alive == false)
            {
                return;
            }

            if (IsAIDecisionRecharging() == true)
            {
                return;
            }

            if (_characterAITemplate.AttackFromRange == true)
            {
                if (_characterMovementController.IsCharacterWithinMeleeRange(_playerMovementController) == false)
                {
                    if (ActivateSecondaryAttack())
                    {
                        _characterMovementController.FaceDirectionOfCharacter(_playerMovementController);
                        _characterMovementController.StopMovement();
                        TriggerAIDecisionRecharge();
                    }
                }
            }
        }


        private void ProcessChaseBehaviour()
        {
            if(_playerCombatController.Alive == false)
            {
                return;
            }

            if (IsAIDecisionRecharging() == true)
            {
                return;
            }

            if (_characterAITemplate.ChasePlayer == true)
            {
                if (_characterMovementController.IsCharacterWithinMeleeRange(_playerMovementController))
                {

                    //_characterMovementController.RotateCharacterXZTowardsPoint(_playerMovementController.gameObject.transform.position);
                    _characterMovementController.FaceDirectionOfCharacter(_playerMovementController);
                    _characterMovementController.StopMovement();
                    if (IsAIDecisionRecharging() == false)
                    {
                        ActivatePrimaryAttack();
                        TriggerAIDecisionRecharge();
                    }

                    return;
                }

                if (_characterMovementController != null)
                {
                    _characterMovementController.MoveToGameObject(PlayerTest);
                }
            }
        }

        private bool ActivatePrimaryAttack()
        {
            return _characterSkillController.ActivateSkill(0);
        }
        private bool ActivateSecondaryAttack()
        {
            return _characterSkillController.ActivateSkill(1);
        }

        private void ProcessAIDecisionRecharge()
        {
            if(_decisionRechargeCounter > 0f)
            {
                _decisionRechargeCounter -= Time.deltaTime;
            }
        }

        private bool IsAIDecisionRecharging()
        {
            return _decisionRechargeCounter > 0f;
        }

        private void TriggerAIDecisionRecharge()
        {
            _decisionRechargeCounter = _decisionRecharge;
        }
    }
}