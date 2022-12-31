using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterState 
    {
        public bool CanMove = true;
        public bool CanAttack = true;
        public bool Damageable = true;
        public bool Healable = true;
        public bool Movable = true;
        public bool ControlableByInput = true;

    }
}