using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterUIController _characterUIController;
        public void Start()
        {
            _characterUIController = GetComponent<CharacterUIController>();
            _characterUIController.ToggleHealthBar(false);
        }
    }
}
