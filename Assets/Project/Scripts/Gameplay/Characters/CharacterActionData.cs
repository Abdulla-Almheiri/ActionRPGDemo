using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterActionData
    {
        public CharacterAction CharacterAction;
        public CharacterState TransitionTo;
        public CharacterStateTransitionData TransitionData;
    }
}
