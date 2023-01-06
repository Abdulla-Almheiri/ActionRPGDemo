using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character combat template", menuName = "Content/Characters/Character Combat Template")]
    public class CharacterCombatTemplate : ScriptableObject
    {
        public bool Hostile = true;
        public float Size = 1f;

        public List<CharacterAttributeData> BaseCharacterAttributes = new List<CharacterAttributeData>();
    }
}