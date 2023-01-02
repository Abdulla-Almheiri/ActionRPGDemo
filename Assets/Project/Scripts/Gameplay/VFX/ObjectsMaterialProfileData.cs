using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.VFX
{
    [System.Serializable]
    public class ObjectsMaterialProfileData 
    {
        public SkillActionElement Element;
        public Color MaterialHighlightColor = Color.clear;
        public float Duration = 1f;
        public AnimationCurve HighlightPowerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    }
}