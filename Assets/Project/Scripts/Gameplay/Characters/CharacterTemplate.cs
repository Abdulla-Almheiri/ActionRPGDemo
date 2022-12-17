using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character template", menuName = "Content/Characters/Character Template")]
    public class CharacterTemplate : ScriptableObject
    {
        public List<CharacterAttribute> CharacterAttributes = new List<CharacterAttribute>();
    }
}