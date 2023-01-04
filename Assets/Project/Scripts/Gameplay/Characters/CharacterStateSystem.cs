using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterStateSystem
    {
        private CharacterState _characterState;

        public CharacterStateSystem(CharacterState characterState)
        {
            _characterState = characterState;
        }
        public bool IsTransitionToStatePossible(CharacterState characterState)
        {
            return false;
        }


    }
}