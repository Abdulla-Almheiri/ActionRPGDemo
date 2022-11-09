using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character
{
    [CreateAssetMenu(fileName ="CharacterTemplate", menuName ="Character/CharacterTemplate")]
    public class CharacterTemplate : ScriptableObject
    {
        public SkinnedMeshRenderer SkinnedMesh;
        public Mesh Mesh;
        public AnimatorOverrideController Animator;
    }
}
