using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character movement template", menuName = "Content/Characters/Character Movement Template")]
    public class CharacterMovementTemplate : ScriptableObject
    {
        public float BaseMovementSpeed = 1f;
        public LayerMask GroundLayer;
    }
}