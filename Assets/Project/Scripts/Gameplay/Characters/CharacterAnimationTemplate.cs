using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character animation template", menuName = "Content/Characters/Character Animation Template")]
    public class CharacterAnimationTemplate: ScriptableObject
    {
        public List<CharacterAnimationData> AnimationData = new List<CharacterAnimationData>();
    }
}