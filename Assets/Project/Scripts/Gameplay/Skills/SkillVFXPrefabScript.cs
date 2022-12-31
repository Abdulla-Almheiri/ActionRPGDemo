using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    public abstract class SkillVFXPrefabScript : MonoBehaviour
    {
        private List<SkillAction> _skillActions;

        public void Initialize(List<SkillAction> skillActions)
        {
            _skillActions = skillActions;

        }
    }
}