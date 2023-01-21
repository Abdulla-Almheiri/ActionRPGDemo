using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.UI
{
    [CreateAssetMenu(fileName = "new combat text profile", menuName = "Content/UI/Floating Combat Text Profile")]
    public class FloatingCombatTextProfile : ScriptableObject
    {
        public FloatingCombatTextTemplate Damage;
        public FloatingCombatTextTemplate DamageTakenAsPlayer;
        public FloatingCombatTextTemplate CriticalDamage;
        public FloatingCombatTextTemplate Healing;
        public FloatingCombatTextTemplate CriticalHealing;
        public FloatingCombatTextTemplate CharacterStatusChange;
        public FloatingCombatTextTemplate HealingRegeneration;
        public FloatingCombatTextTemplate EnergyRegeneration;
    }
}