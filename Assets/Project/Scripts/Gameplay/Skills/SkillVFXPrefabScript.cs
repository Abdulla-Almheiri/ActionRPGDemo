using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    public abstract class SkillVFXPrefabScript : MonoBehaviour
    {
        private List<SkillActionData> _skillActions;

        public void Initialize(List<SkillActionData> skillActions)
        {
            _skillActions = skillActions;

        }
    }
}