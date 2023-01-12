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
        [Tooltip("Reduced by HitRecovery. Increased by Skill's HitPower when hit.")]
        public float BaseHitFrameDuration = 0.3f;

        [Space(10)]
        [Header("Core Combat Attributes")]
        [Space(5)]
        public CharacterAttributeData Health;
        public CharacterAttributeData Energy;

        public List<CharacterAttributeData> PrimaryCharacterAttributes = new List<CharacterAttributeData>();
        public List<CharacterAttributeData> SecondaryCharacterAttributes = new List<CharacterAttributeData>();
    }
}