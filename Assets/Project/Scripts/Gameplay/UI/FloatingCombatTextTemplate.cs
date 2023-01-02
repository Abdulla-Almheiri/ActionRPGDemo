using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.UI
{
    [CreateAssetMenu(fileName = "new floating text template", menuName = "Content/UI/FloatingCombatText Template")]
    public class FloatingCombatTextTemplate : ScriptableObject
    {
        public Color Color = Color.white;
        public List<FloatingCombatTextAnimationData> PropertiesToAnimate = new List<FloatingCombatTextAnimationData>();

    }
}