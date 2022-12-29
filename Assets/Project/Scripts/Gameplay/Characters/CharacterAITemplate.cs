using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character AI template", menuName = "Content/Characters/Character AI Template")]
    public class CharacterAITemplate : ScriptableObject
    {
        public bool ChasePlayer = true;
        public bool AttackPlayer = true;

    }
}