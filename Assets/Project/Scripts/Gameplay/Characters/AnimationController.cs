using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class AnimationController : MonoBehaviour
    {
        
        protected MovementController _movementController;
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
        }

        protected void Initialize()
        {
            _movementController = GetComponent<MovementController>();
            _animator = GetComponent<Animator>();
        }

        protected void HandleRunningAnimation()
        {
            if(_animator == null || _movementController == null)
            {
                return;
            }

            var currentSpeed = _movementController.GetCurrentMovementSpeed();
            Debug.Log("Current speed =   " + currentSpeed);
            _animator.SetFloat("MovementSpeed", currentSpeed);
        }
    }
}