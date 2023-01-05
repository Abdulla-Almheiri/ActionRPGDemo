using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character action", menuName = "Content/Characters/Character Action")]
    public class CharacterAction : ScriptableObject
    {
        public CharacterState TransitionToState;
        public bool RemovesAnyCondition = false;
        public List<CharacterCondition> RemoveCharacterConditions = new List<CharacterCondition>();
    }
}