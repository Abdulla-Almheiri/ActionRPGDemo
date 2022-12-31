using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character state", menuName = "Content/Characters/Character State")]
    public class CharacterStateTemplate : ScriptableObject
    {
        public CharacterState Characterstate;

    }
}