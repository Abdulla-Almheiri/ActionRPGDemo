using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.UI
{
    [System.Serializable]
    public class FloatingCombatTextAnimationData 
    {
        public FloatingCombatTextAnimationDataPropertyType PropertyToAnimate = FloatingCombatTextAnimationDataPropertyType.RelativePositionY;
        public float DurationInSeconds = 1f;
        public float CurveMultiplier = 10f;
        public AnimationCurve AnimationCurve;

    }

    public enum FloatingCombatTextAnimationDataPropertyType
    {
        RelativePositionX,
        RelativePositionY,
        Scale,
        Transparency
    }
}