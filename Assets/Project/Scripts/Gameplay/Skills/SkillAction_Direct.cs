using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillAction_Direct : SkillAction
    {
        [SerializeField]
        protected SkillActionCoefficient _damage;
        [SerializeField]
        protected SkillActionCoefficient _healing;
        [SerializeField]
        protected SkillActionCoefficient _sacrificeDamage;
        [SerializeField]
        protected SkillActionCoefficient _friendlyFire;
        [SerializeField]
        protected SkillActionCoefficient _drainResource;

        public override void Apply(ISkillActionReceiver activator, ISkillActionReceiver receiver)
        {
            throw new System.NotImplementedException();
        }
    }
}