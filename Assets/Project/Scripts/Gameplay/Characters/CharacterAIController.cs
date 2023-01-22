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
        private float _decisionRecharge = 1f;
        private float _decisionRechargeCounter = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            ProcessCharacterAI();
            ProcessAIDecisionRecharge();
        }

        public void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterSkillController = GetComponent<CharacterSkillController>();
            _playerCombatController = PlayerTest.GetComponent<CharacterCombatController>();
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