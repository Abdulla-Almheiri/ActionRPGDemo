using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillTemplateSkillActionEntry
    {
        public SkillAction<SkillActionData> SkillAction;
        public SkillActionData Data;
    }
}