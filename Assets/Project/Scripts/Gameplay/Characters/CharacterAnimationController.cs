using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAnimationController : MonoBehaviour
    {
        public CharacterAnimationTemplate AnimationTemplateTest;
        protected CharacterMovementController _movementController;
        protected CharacterSkillController _skillController;
        protected CharacterVFXController _characterVFXController;
        private CharacterStateController _characterStateController;
        private CharacterCombatController _characterCombatController;
        private string _lastAnimationPlayed1;
        protected Animator _animator;

        private CharacterAnimationData _lastAnimationPlayed;

        private float _baseAnimatorPlaybackSpeed = 1f;
        private float _currentAnimatorPlaybackSpeed = 1f;
        private float _baseAnimatorPlaybackSpeedModifier = 1f;
        private float _currentAnimatorPlaybackSpeedModifier = 1f;

        private float _timeSinceLastAnimationStarted = 0f;

        private float _remainingDurationAnimatorPlaybackSpeedChanged = 0f;

        //private AnimatorPlaybackSystem _animatorPlaybackSystem;

        void Start()
        {
            
            Initialize();
        }
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


        private void Initialize()
        {
            _movementController = GetComponent<CharacterMovementController>();
            _animator = GetComponent<Animator>();
            _skillController = GetComponent<CharacterSkillController>();
            _characterVFXController = GetComponent<CharacterVFXController>();
            _characterStateController = GetComponent<CharacterStateController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
            //_lastAnimationPlayed = 
            SubscribeToCharacterStateControllerOnCharacterStateChangeEvent();
        }
        public void TriggerAttackAnimation()
        {
            _animator.SetTrigger("Attack1");
        }

        public void TriggerHitAnimation()
        {
            _animator.SetTrigger("Hit");
        }
        public float GetCurrentAnimatorPlaybackSpeed()
        {
            return _animator.speed;
        }
        public void PlayCharacterAnimationFromCharacterState(CharacterState characterState, float blendDuration)
        {
            var data = GetCharacterAnimationDataFromTemplate(characterState);
            PlayCharacterAnimationFromCharacterAnimationData(data, blendDuration);
        }
        public void PlayCharacterAnimationFromCharacterAnimationData(CharacterAnimationData characterAnimationData, float blendDuration = 0f)
        {
            if (_animator == null || characterAnimationData == null || characterAnimationData.Animation == null)
            {
                return;
            }

            var looping = _animator.GetCurrentAnimatorStateInfo(0).loop;
            _animator.CrossFade(characterAnimationData.Animation.AnimationHash, blendDuration);

            SetAnimatorPlaybackSpeed(characterAnimationData.GetFinalAnimationSpeed(_characterCombatController));

        }
        public CharacterAnimationData GetCharacterAnimationDataFromTemplate(CharacterState characterState)
        {
            if(characterState == null || characterState.CharacterAnimation == null)
            {
                return null;
            }
            
            var data = AnimationTemplateTest.AnimationData.Find(x => x.Animation == characterState.CharacterAnimation);
            return data;

        }

        public CharacterAttributeScalingData GetCharacterAttributeScalingDataFromTemplate(CharacterState characterState)
        {
            var scalingData = AnimationTemplateTest.AnimationData.Find(x => x.CharacterAttributeAnimationScalingData.CharacterAttribute == characterState);
            return scalingData.CharacterAttributeAnimationScalingData;
        }
        public void RepeatLastAnimationPlayed(float blendDuration = 0f)
        {
            if(_lastAnimationPlayed.IsLooping == true)
            {
                return;
            } else
            {
                PlayCharacterAnimationFromCharacterAnimationData(_lastAnimationPlayed, blendDuration);
            }
        }

        private void CrossFadeToTargetAnimation(CharacterAnimationData targetAnimation, float blendDuration)
        {
            blendDuration = Mathf.Max(0f, blendDuration);
            _animator.CrossFade(targetAnimation.Animation.AnimationHash,blendDuration);
        }
        private void CrossFadeAnimatorPlaybackSpeed(float targetValue, float duration)
        {
            StartCoroutine(CrossFadeAnimatorPlaybackSpeedCO(targetValue, duration));
        }

        private IEnumerator CrossFadeAnimatorPlaybackSpeedCO(float targetValue, float duration = 1f)
        {
            var wait = new WaitForEndOfFrame();
            float progress = 0f;
            float difference = targetValue - _currentAnimatorPlaybackSpeed;
            while(progress < duration)
            {
                progress += Time.deltaTime;
                SetAnimatorPlaybackSpeed(_currentAnimatorPlaybackSpeed + (progress / duration * difference));
                yield return wait;
            }
            SetAnimatorPlaybackSpeed(targetValue);
        }
        public void AddModifierToAnimatorPlaybackSpeedTemporary(float speedModifier = 1f, float duration = 0f)
        {
            _currentAnimatorPlaybackSpeedModifier += speedModifier;
        }
        private void SetAnimatorPlaybackSpeedModifierTemporary(float speedModifier = 1f, float duration = 0f)
        {
            _currentAnimatorPlaybackSpeedModifier = speedModifier;
        }
        private void SetAnimatorPlaybackSpeedTemporary(float newSpeed, float duration = 0f)
        {
            SetAnimatorPlaybackSpeed(newSpeed);
            _remainingDurationAnimatorPlaybackSpeedChanged = Mathf.Max(0f, duration);
        }

        private void ProcessAnimatorPlaybackSpeedReset()
        {
            if(_currentAnimatorPlaybackSpeed == _baseAnimatorPlaybackSpeed)
            {
                return;
            } 
            else if(_remainingDurationAnimatorPlaybackSpeedChanged > 0f)
            {
                _remainingDurationAnimatorPlaybackSpeedChanged -= Time.deltaTime;
            } else
            {
                SetAnimatorPlaybackSpeed(_baseAnimatorPlaybackSpeed);
            }
        }
        private void ProcessAnimatorPlaybackSpeedModifierReset(float speedModifier)
        {

        }

        private void SetAnimatorPlaybackSpeed(float newSpeed)
        {
            _currentAnimatorPlaybackSpeed = newSpeed;
            _animator.speed = newSpeed;
        }
        private void HandleRunningAnimation()
        {
            if(_animator == null || _movementController == null)
            {
                return;
            }

            var currentSpeed = _movementController.GetCurrentMovementSpeed();
            //Debug.Log("Current speed =   " + currentSpeed);
            //_animator.SetFloat("MovementSpeed", currentSpeed);
        }
        private float GetBaseAnimationClipDuration(CharacterAnimationData animationData)
        {
            Debug.Log("Clip " + animationData.AnimationClip.name + " length is          :     " + animationData.AnimationClip.length);
            return animationData.AnimationClip.length;
        }

        private float GetTrimmedAnimationDuration(CharacterAnimationData animationData)
        {
            return GetBaseAnimationClipDuration(animationData) * animationData.TrimAnimationFrom;
 
        }

        private float GetModifiedAnimationDuration(CharacterAnimationData animationData)
        {
            if(animationData.BaseAnimationSpeed == 0f)
            {
                return 0f;
            }
            return GetTrimmedAnimationDuration(animationData) / animationData.GetFinalAnimationSpeed(_characterCombatController);
        }
        public float GetActualAnimationDuration(CharacterAnimationData animationData)
        {
            float duration = GetModifiedAnimationDuration(animationData);
            duration /= _currentAnimatorPlaybackSpeedModifier;
            return duration;
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
            if (_lastAnimationPlayed != null && _lastAnimationPlayed.Animation == newCharacterState)
            {
                _animator.Play(_characterStateController.CharacterStatesProfile.Idle.CharacterAnimation.AnimationHash);
            }
            _animator.CrossFade(newCharacterState.CharacterAnimation.AnimationHash, delay);
            _lastAnimationPlayed = GetCharacterAnimationDataFromTemplate(newCharacterState);
            //Debug.Log("Playing animation  : " + newCharacterState.CharacterAnimation.AnimationHash);
            if(newCharacterState.AutomaticStateTransitionData != null)
            {
                RequestCharacterStateChangeToNextStateAfterAnimationIsFinished(newCharacterState.AutomaticStateTransitionData.State, delay);
            }
        }

        private void RequestCharacterStateChangeToNextStateAfterAnimationIsFinished(CharacterState newCharacterState, float delay)
        {
            float currentAnimationDelay = _animator.GetCurrentAnimatorStateInfo(0).length;
            _characterStateController.QueueNextCharacterState(newCharacterState, currentAnimationDelay);
        }

    }
}