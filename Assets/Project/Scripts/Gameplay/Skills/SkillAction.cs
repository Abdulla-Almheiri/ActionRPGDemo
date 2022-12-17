using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using UnityEngine.UI;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public abstract class SkillAction 
    {
        public string Name;
        public Image Icon;
        //public SkillActionCoefficient Coefficient;

        //public void TriggerByAndTo(character)

        public abstract void Apply(ISkillActionReceiver activator, ISkillActionReceiver receiver);
    }
}