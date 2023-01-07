using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character animation layer", menuName = "Content/Characters/Character Animation Layer")]
    public class CharacterAnimationLayer : ScriptableObject 
    {
        public int AnimationLayerIndex { private set; get; }
        public void OnEnable()
        {
            AnimationLayerIndex = Animator.StringToHash(name);
            if (AnimationLayerIndex == -1)
            {
                Debug.Log("Layer " + name + " doesn't exist." + AnimationLayerIndex);
            }
        }

    }
}
