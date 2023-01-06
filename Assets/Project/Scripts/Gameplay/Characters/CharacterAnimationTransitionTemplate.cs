using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character animation transition template", menuName = "Content/Characters/Character Animation Transition Template")]
    public class CharacterAnimationTransitionTemplate : ScriptableObject
    {
        public List<CharacterAnimationTransitionData> Transitions = new List<CharacterAnimationTransitionData>();
    }
}