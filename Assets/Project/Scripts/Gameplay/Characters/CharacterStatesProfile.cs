using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character states profile", menuName = "Content/Characters/Character States Profile")]
    public class CharacterStatesProfile : ScriptableObject
    {
        public CharacterState Idle;
        public CharacterState Walking;
        public CharacterState ExecutingBasicAttack;
        public CharacterState ExecutingSpecialAttack;
        public CharacterState InHitFrame;
        public CharacterState Interacting;
        public CharacterState Dead;
        public CharacterState Dying;
        public CharacterState Reviving;

    }
}