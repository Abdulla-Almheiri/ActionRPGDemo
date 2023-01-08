using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterRootMotionCorrectionScript : MonoBehaviour
    {
        private Animator _animator;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponentInParent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //transform.parent.transform.position += _animator.deltaPosition;
        }

        /*private void OnAnimatorMove()
        {
            Debug.Log("ANIMATOR MOVED");
            transform.transform.position -= _animator.deltaPosition;
        }*/
    }
}