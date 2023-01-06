using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character animation", menuName = "Content/Characters/Character Animation")]
    public class CharacterAnimation : ScriptableObject
    {
        public int AnimationHash { private set; get; }
        public void OnEnable()
        {
            AnimationHash = Animator.StringToHash(name);
            //Debug.Log("Name is  :  " + name + ".  And hash is   :   " + AnimationHash);
        }
    }
}