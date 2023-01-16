using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAnimationController : MonoBehaviour
    {
        public Transform child;
        public CharacterAnimationTemplate AnimationTemplateTest;
        protected CharacterMovementController _movementController;
        protected CharacterSkillController _skillController;
        protected CharacterVFXController _characterVFXController;
        private CharacterStateController _characterStateController;
        private CharacterCombatController _characterCombatController;
        private CharacterMovementController _characterMovementController;

        public Animator Animator { private set; get; }

        private CharacterAnimationData _lastAnimationPlayed;

        private float _baseAnimatorPlaybackSpeed = 1f;
        private float _currentAnimatorPlaybackSpeed = 1f;
        private float _baseAnimatorPlaybackSpeedModifier = 1f;
        private float _currentAnimatorPlaybackSpeedModifier = 1f;

        private float _timeSinceLastAnimationStarted = 0f;

        private float _remainingDurationAnimatorPlaybackSpeedChanged = 0f;


        void Start()
        { 
            Initialize();
        }


        public void TriggerAnimation(string actionName, bool stopMovement = true)
        {
            Animator.SetTrigger(actionName);
            if (stopMovement == true)
            {
                _characterMovementController.StopMovement();
                //_characterMovementController.RotateCharacterInMouseDirection();
            }
        }

        public bool TriggerAnimation(CharacterAnimation characterAnimation)
        {
            if(characterAnimation == null || Animator == null)
            {
                return false;
            }

            Animator.SetTrigger(characterAnimation.name);
            if (characterAnimation.StopMovement == true)
            {
                _characterMovementController.StopMovement();
            }

            if(characterAnimation.FaceMousePoint == true)
            {
               // _characterMovementController.RotateCharacterInMouseDirection();
            }
            return true;
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
            Animator = GetComponent<Animator>();
            _skillController = GetComponent<CharacterSkillController>();
            _characterVFXController = GetComponent<CharacterVFXController>();
            _characterStateController = GetComponent<CharacterStateController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
            _characterMovementController = GetComponent<CharacterMovementController>();
            //_lastAnimationPlayed = 
            SubscribeToCharacterStateControllerOnCharacterStateChangeEvent();
        }

        public void TriggerHitAnimation()
        {
            Animator.SetTrigger("Hit");
        }
        public float GetCurrentAnimatorPlaybackSpeed()
        {
            return Animator.speed;
        }
        public void PlayCharacterAnimationFromCharacterState(CharacterState characterState, float blendDuration)
        {
            var data = GetCharacterAnimationDataFromTemplate(characterState);
            PlayCharacterAnimationFromCharacterAnimationData(data, blendDuration);
        }
        public void PlayCharacterAnimationFromCharacterAnimationData(CharacterAnimationData characterAnimationData, float blendDuration = 0f)
        {
            if (Animator == null || characterAnimationData == null || characterAnimationData.Animation == null)
            {
                return;
            }

            var looping = Animator.GetCurrentAnimatorStateInfo(0).loop;
            Animator.CrossFade(characterAnimationData.Animation.AnimationHash, blendDuration);

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
            Animator.CrossFade(targetAnimation.Animation.AnimationHash,blendDuration);
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
            Animator.speed = newSpeed;
        }
        private float GetBaseAnimationClipDuration(CharacterAnimationData animationData)
        {
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
                Animator.Play(_characterStateController.CharacterStatesProfile.Idle.CharacterAnimation.AnimationHash);
            }
            Animator.CrossFade(newCharacterState.CharacterAnimation.AnimationHash, delay);
            _lastAnimationPlayed = GetCharacterAnimationDataFromTemplate(newCharacterState);
            //Debug.Log("Playing animation  : " + newCharacterState.CharacterAnimation.AnimationHash);
            if(newCharacterState.AutomaticStateTransitionData != null)
            {
                //Fix here
                //RequestCharacterStateChangeToNextStateAfterAnimationIsFinished(newCharacterState.AutomaticStateTransitionData.State, delay);
            }
        }

        private void RequestCharacterStateChangeToNextStateAfterAnimationIsFinished(CharacterState newCharacterState, float delay)
        {
            float currentAnimationDelay = Animator.GetCurrentAnimatorStateInfo(0).length;
            _characterStateController.QueueNextCharacterState(newCharacterState, currentAnimationDelay);
        }

    }
}