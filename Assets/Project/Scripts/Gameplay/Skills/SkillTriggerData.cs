using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillTriggerData
    {
        public float TriggerChance = 100f;
        public SkillTrigger SkillTrigger;

    }
}