using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterAnimationTransitionData
    {
        public CharacterState CharacterState;
        public CharacterState NextState;
        public CharacterAnimationData TransitionAnimationData;

    }
}