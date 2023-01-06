using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character combat profile", menuName = "Content/Characters/Character Combat Profile")]
    public class CharacterCombatProfile : ScriptableObject
    {
        public CharacterActionCommandData BasicAttack;
        public CharacterActionCommandData SpecialAttack;
        public CharacterActionCommandData Hit;

    }
}