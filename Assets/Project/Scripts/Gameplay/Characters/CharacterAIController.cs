using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAIController : MonoBehaviour
    {
        public GameObject PlayerTest;
        private CharacterMovementController _playerMovementController;
        [SerializeField]
        protected CharacterAITemplate _characterAITemplate;
        protected CharacterMovementController _characterMovementController;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            ProcessCharacterAI();
        }

        public void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
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

        }

        private void ProcessChaseBehaviour()
        {
            if (_characterAITemplate.ChasePlayer == true)
            {
                if (_characterMovementController.IsCharacterWithinMeleeRange(_playerMovementController))
                {
                    //Debug.Log("CHARACTER IS WITHIN MElEE RANGE");
                    //_characterMovementController.StopMovementAndRequestStateChangeToIdle();
                    return;
                }

                if (_characterMovementController != null)
                {
                    _characterMovementController.MoveToGameObject(PlayerTest);
                }
            }
        }
    }
}