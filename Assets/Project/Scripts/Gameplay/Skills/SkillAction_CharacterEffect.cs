using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [System.Serializable]
    public class SkillAction_CharacterEffect 
    {
        [SerializeField]
        protected float _duration;

        [SerializeField]
        protected CharacterAttribute _attribute;

    }
}