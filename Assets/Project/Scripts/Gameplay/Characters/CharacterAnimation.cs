using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character animation", menuName = "Content/Characters/Character Animation")]
    public class CharacterAnimation : ScriptableObject
    {
        [field:SerializeField]
        public bool StopMovement { private set; get; } = true;

        [field: SerializeField]
        public bool FaceMousePoint { private set; get; } = true;
        public int AnimationHash { private set; get; }
        public void OnEnable()
        {
            AnimationHash = Animator.StringToHash(name);
        }
    }
}