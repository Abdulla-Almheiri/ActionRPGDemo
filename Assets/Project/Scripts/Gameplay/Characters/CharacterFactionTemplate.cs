using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character faction template", menuName = "Content/Characters/Character Faction Template")]
    public class CharacterFactionTemplate : ScriptableObject
    {
        public List<CharacterFaction> Friendly = new List<CharacterFaction>();
        public List<CharacterFaction> Neutral = new List<CharacterFaction>();
        public List<CharacterFaction> Hostile= new List<CharacterFaction>();
    }
}