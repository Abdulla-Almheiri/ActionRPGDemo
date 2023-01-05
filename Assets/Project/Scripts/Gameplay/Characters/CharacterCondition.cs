using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character condition", menuName = "Content/Characters/Character Condition")]
    public class CharacterCondition : ScriptableObject
    {
        public bool CanReceiveHealing = true;
        public bool CanReceiveDamage = true;

        public List<CharacterAction> ActionsNotAllowed = new List<CharacterAction>();
    }
}