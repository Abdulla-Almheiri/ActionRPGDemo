using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character AI template", menuName = "Content/Characters/Character AI Template")]
    public class CharacterAITemplate : ScriptableObject
    {
        public float PlayerDetectionRange = 10f;
        public bool ChasePlayer = true;
        public bool AttackPlayer = true;
        public bool AttackFromRange = true;
    }
}