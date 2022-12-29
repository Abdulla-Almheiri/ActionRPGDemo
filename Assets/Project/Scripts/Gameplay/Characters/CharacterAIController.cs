using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAIController : MonoBehaviour
    {
        public GameObject PlayerTest;
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
        }

        private void ProcessCharacterAI()
        {
            if(_characterAITemplate == null)
            {
                return;
            }

            if(_characterAITemplate.ChasePlayer == true)
            {
                if(_characterMovementController != null)
                {
                    _characterMovementController.MoveToGameObject(PlayerTest);
                }
            }
        }
    }
}