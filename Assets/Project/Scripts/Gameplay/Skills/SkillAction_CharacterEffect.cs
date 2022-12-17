using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillAction_CharacterEffect : SkillAction
    {
        [SerializeField]
        protected float _duration;

        [SerializeField]
        protected Attribute _attribute;

        public override void Apply(ISkillActionReceiver activator, ISkillActionReceiver receiver)
        {
            throw new System.NotImplementedException();
        }
    }
}