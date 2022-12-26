using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class AnimationController : MonoBehaviour
    {
        
        protected MovementController _movementController;
        protected SkillController _skillController;
        protected CharacterVFXController _characterVFXController;
        protected Animator _animator;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            HandleRunningAnimation();

            if(Input.GetKeyUp(KeyCode.Alpha1) == true)
            {
                _animator.SetTrigger("Attack1");
                //TEST
                _skillController.SpawnSkillVFXTest();
                Debug.Log("Attack triggered");
            }

            if (Input.GetKeyUp(KeyCode.F) == true)
            {
                _animator.SetTrigger("Death");
                Debug.Log("Death triggered");
            }

            if (Input.GetKeyUp(KeyCode.H) == true)
            {
                _animator.SetTrigger("Hit");
                Debug.Log("Hit triggered");
            }
        }

        protected void Initialize()
        {
            _movementController = GetComponent<MovementController>();
            _animator = GetComponent<Animator>();
            _skillController = GetComponent<SkillController>();
            _characterVFXController = GetComponent<CharacterVFXController>();
        }

        protected void HandleRunningAnimation()
        {
            if(_animator == null || _movementController == null)
            {
                return;
            }

            var currentSpeed = _movementController.GetCurrentMovementSpeed();
            //Debug.Log("Current speed =   " + currentSpeed);
            _animator.SetFloat("MovementSpeed", currentSpeed);
        }
    }
}