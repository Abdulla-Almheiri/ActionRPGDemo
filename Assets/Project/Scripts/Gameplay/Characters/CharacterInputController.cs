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
            if (Input.GetMouseButton(0) == true)
            {
                _characterMovementController.MoveToMousePosition();

            }

            if(Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                _characterSkillController.ActivateSkill(1);
            }
        }

        private void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterSkillController = GetComponent<CharacterSkillController>();
        }
    }
}