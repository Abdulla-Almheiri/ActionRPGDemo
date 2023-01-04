using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAnimationController : MonoBehaviour
    {
        public CharacterAnimationTemplate AnimationTemplateTest;
        protected CharacterMovementController _movementController;
        protected CharacterSkillController _skillController;
        protected CharacterVFXController _characterVFXController;
        private CharacterStateController _characterStateController;
        private string _lastAnimationPlayed;
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
                //_animator.SetTrigger("Attack1");
                //TEST
                //_skillController?.SpawnSkillVFXTest();
                //Debug.Log("Attack triggered");
            }

            if (Input.GetKeyUp(KeyCode.F) == true)
            {
                _animator.SetTrigger("Death");
                //Debug.Log("Death triggered");
            }

            if (Input.GetKeyUp(KeyCode.H) == true)
            {
                _animator.SetTrigger("Hit");
                //Debug.Log("Hit triggered");
            }

            if (Input.GetKeyUp(KeyCode.A))
            {

                _animator.CrossFade("Base Layer.Attack1", 0.2f);
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                Debug.Log("Animation HASH  :  " + Animator.StringToHash("Base Layer.Hit"));
                _animator.CrossFade(Animator.StringToHash("Base Layer.Hit"), 0.2f);
            }

            if (Input.GetKeyUp(KeyCode.T))
            {
                if (AnimationTemplateTest != null)
                {
                   
                    GetComponent<Animation>().clip = AnimationTemplateTest.Animation;
                    GetComponent<Animation>().CrossFade(AnimationTemplateTest.Animation.name, 0.5f);
                }
            }
        }

        protected void Initialize()
        {
            _movementController = GetComponent<CharacterMovementController>();
            _animator = GetComponent<Animator>();
            _skillController = GetComponent<CharacterSkillController>();
            _characterVFXController = GetComponent<CharacterVFXController>();
            _characterStateController = GetComponent<CharacterStateController>();
            SubscribeToCharacterStateControllerOnCharacterStateChangeEvent();
        }

        protected void HandleRunningAnimation()
        {
            if(_animator == null || _movementController == null)
            {
                return;
            }

            var currentSpeed = _movementController.GetCurrentMovementSpeed();
            //Debug.Log("Current speed =   " + currentSpeed);
            //_animator.SetFloat("MovementSpeed", currentSpeed);
        }

        public void TriggerAttackAnimation()
        {
            _animator.SetTrigger("Attack1");
        }

        public void TriggerHitAnimation()
        {
            _animator.SetTrigger("Hit");
        }

        private void SubscribeToCharacterStateControllerOnCharacterStateChangeEvent()
        {
            _characterStateController?.SubscribeToCharacterStateChanged(OnCharacterStateChanged);
        }

        private void UnsubscribeToCharacterStateControllerOnCharacterStateChangeEvent()
        {
            _characterStateController?.UnsubscribeToCharacterStateChanged(OnCharacterStateChanged);
        }
        private void OnCharacterStateChanged(CharacterState newCharacterState, float delay = 0f)
        {
            PlayAnimationCharacterStateTransition(newCharacterState, delay);
        }

        private void PlayAnimationCharacterStateTransition(CharacterState newCharacterState, float delay = 0f)
        {
            if (_lastAnimationPlayed == newCharacterState.AnimationName)
            {
                _animator.Play(_characterStateController.CharacterStatesProfile.Idle.AnimationName);
            }
            _animator.CrossFade(newCharacterState.AnimationName, delay);
            _lastAnimationPlayed = newCharacterState.AnimationName;
            Debug.Log("Playing animation  : " + newCharacterState.AnimationName);
            if(newCharacterState.NextCharacterStateAfterAnimationFinished != null)
            {
                RequestCharacterStateChangeToNextStateAfterAnimationIsFinished(newCharacterState.NextCharacterStateAfterAnimationFinished, delay);
            }
        }

        private void RequestCharacterStateChangeToNextStateAfterAnimationIsFinished(CharacterState newCharacterState, float delay)
        {
            float currentAnimationDelay = _animator.GetCurrentAnimatorStateInfo(0).length;
            _characterStateController.QueueNextCharacterState(newCharacterState, currentAnimationDelay);
        }
        public void OnEnable()
        {
            SubscribeToCharacterStateControllerOnCharacterStateChangeEvent();
        }

        public void OnDisable()
        {
            UnsubscribeToCharacterStateControllerOnCharacterStateChangeEvent();
        }

        public void OnDestroy()
        {
            UnsubscribeToCharacterStateControllerOnCharacterStateChangeEvent();
        }
    }
}