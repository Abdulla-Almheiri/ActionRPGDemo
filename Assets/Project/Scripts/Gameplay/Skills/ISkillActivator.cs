using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    public interface ISkillActivator
    {
        void ActivateSkill(Skill skill);
    }
}