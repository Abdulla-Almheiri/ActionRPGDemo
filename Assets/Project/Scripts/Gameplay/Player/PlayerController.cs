using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterAnimationController _characterAnimationController;
        private CharacterUIController _characterUIController;

        public void Update()
        {
        }

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _characterUIController = GetComponent<CharacterUIController>();
            _characterUIController.ToggleHealthBar(false);
            _characterAnimationController = GetComponent<CharacterAnimationController>();
        }


    }
}
