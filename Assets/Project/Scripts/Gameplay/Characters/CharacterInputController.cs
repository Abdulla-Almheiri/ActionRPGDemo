using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterInputController : MonoBehaviour
    {
        private CharacterMovementController _characterMovementController;
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
        }

        private void Initialize()
        {
            _characterMovementController = GetComponent<CharacterMovementController>();
        }
    }
}