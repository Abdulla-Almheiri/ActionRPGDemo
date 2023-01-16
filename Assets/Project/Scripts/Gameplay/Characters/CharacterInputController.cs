using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterInputController : MonoBehaviour
    {
        private CharacterMovementController _characterMovementController;
        private CharacterSkillController _characterSkillController;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            ListenForInput();
        }

        private void ListenForInput()
        {
            if (Input.GetMouseButton(0) == true)
            {
                _characterMovementController.MoveToMousePosition();

            }

            if (Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                _characterSkillController.ActivateSkill(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) == true)
            {
                _characterSkillController.ActivateSkill(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) == true)
            {
                _characterSkillController.ActivateSkill(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) == true)
            {
                _characterSkillController.ActivateSkill(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) == true)
            {
                _characterSkillController.ActivateSkill(4);
            }
        }
        private void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterSkillController = GetComponent<CharacterSkillController>();
        }

    }
}