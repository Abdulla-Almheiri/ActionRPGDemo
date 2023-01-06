using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterStateController : MonoBehaviour
    {
        [field:SerializeField]
        public CharacterStatesProfile CharacterStatesProfile { private set; get; }
        public delegate void CharacterStateChanged(CharacterState newCharacterState, float delay = 0f);

        private event CharacterStateChanged OnCharacterStateChanged;
        private CharacterState _currentCharacterState;
        private CharacterState _initialCharacterstate;
        private float _currentStateElapsedTime = 0f;

        private Queue<CharacterStateTransitionData> _queuedCharacterStateChangeCommands = new Queue<CharacterStateTransitionData>();

        private float[] _currentStateDurations;

        private CharacterAnimationController _characterAnimationController;

        private Dictionary<CharacterAction, float> _unallowedCharacterActions = new Dictionary<CharacterAction, float>();

        public void Start()
        {
            Initialize(CharacterStatesProfile.Idle);
        }
        
        public void Update()
        {
            ProcessCurrentCharacterStateElapsedTime();

            if(Input.GetKeyUp(KeyCode.S))
            {
                SetCurrentCharacterStateWithAnyTransitionType(CharacterStatesProfile.ExecutingBasicAttack);
            }
        }


        private void Initialize(CharacterState initialCharacterState)
        {
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            //_currentCharacterState = initialCharacterState;
            _initialCharacterstate = initialCharacterState;

            _currentStateDurations = new float[5];
            AttemptCharacterStateChange(initialCharacterState);
            ResetStateDurations();
            //_currentState = new CharacterState(true, true, true, true, true, true);
        }
        public CharacterState GetCurrentCharacterState()
        {
            return _currentCharacterState;
        }
        public bool AttemptCharacterStateChange(CharacterState newCharacterState, float duration = 0f)
        {
            if(IsCurrentStateTemporary() == true)
            {
                //There is a temporary state

                //if the same state then just update duration
                if(GetCurrentCharacterState() == newCharacterState)
                {
                    AddToCurrentCharacterStateDurationIfTemporary(duration);
                    //_characterAnimationController.PlayCharacterAnimationFromCharacterState(newCharacterState, 0f);
                    return true;
                } else if(duration == 0f)
                {
                    ChangeCharacterStateAndTriggerEvent(newCharacterState);
                    return true;
                } else
                {
                    ChangeCharacterStateAndTriggerEvent(newCharacterState);
                    return true;
                }

            } 
            return false;

            //SetCurrentCharacterStateWithAnyTransitionType
        }
        public bool SetCurrentCharacterStateIfTransitionIsImmediateOnly(CharacterState newCharacterState)
        {
            if (IsImmediateCharacterStateTransitionPossible(newCharacterState) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState);
            }

            ResetCurrentCharacterStateElapsedTime();
            return true;
        }
        public bool SetCurrentCharacterStateIfTransitionHasDelayOnly(CharacterState newCharacterState)
        {
            if (IsCharacterStateTransitionPossibleAfterFixedDelay(newCharacterState, out float delay) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState, delay);
                return true;
            }
            else if (IsCharacterStateTransitionPossibleAfterPercentageOfAnimationDelay(newCharacterState, out float animationDelay) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState, animationDelay);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetCurrentCharacterStateWithAnyTransitionType(CharacterState newCharacterState)
        {
            if (SetCurrentCharacterStateIfTransitionIsImmediateOnly(newCharacterState) == true)
            {
                return true;
            }
            else if (SetCurrentCharacterStateIfTransitionHasDelayOnly(newCharacterState) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetCurrentCharacterStateTransitionForcedOnly(CharacterState newCharacterState)
        {
            if (IsCharacterStateTransitionPossibleIfForced(newCharacterState) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void QueueNextCharacterState(CharacterState newCharacterState, float delay)
        {
            /*if(SetCurrentCharacterStateIfTransitionIsImmediateOnly(newCharacterState) == true)
            {
                return;
            }*/
            StopAllCoroutines();
            StartCoroutine(QueueNextCharacterStateCO(newCharacterState, delay));
        }
        public void SubscribeToCharacterStateChanged(CharacterStateChanged subscriber)
        {
            OnCharacterStateChanged += subscriber;
        }
        public void UnsubscribeToCharacterStateChanged(CharacterStateChanged unsubscriber)
        {
            OnCharacterStateChanged -= unsubscriber;
        }

        private float GetActualDelayDurationOfTransition(CharacterState nextState, CharacterState previousState = null)
        {
            if(nextState == null)
            {
                return 0f;
            }

            if(previousState == null)
            {
                previousState = _currentCharacterState;
            }

            if(previousState == null)
            {
                return 0f;
            }

            // FIX HERE NOW?


            var state = previousState.Transitions.Find(x=> x.TransitionData.State == nextState);
            if(state == null)
            {
                return 0f;
            }

            var data = _characterAnimationController.GetCharacterAnimationDataFromTemplate(nextState);
            var transitionDelay = state.TransitionData.ActualDelayInSeconds(_characterAnimationController);
            Debug.Log("TRANSITION DELAY IS :          " + transitionDelay);
            return transitionDelay;

        }
        private void ExtendCurrentCharacterStateDurationIfTemporary(float newDuration)
        {
            if(IsCurrentStateTemporary() == true)
            {
                _currentStateElapsedTime = Mathf.Max(_currentStateElapsedTime, newDuration);
            }
        }
        private void AddToCurrentCharacterStateDurationIfTemporary(float addedDuration)
        {
            if(addedDuration <= 0f)
            {
                return;
            }

            if (IsCurrentStateTemporary() == true)
            {
                _currentStateElapsedTime += addedDuration;
            }
        }
        private bool IsCurrentStateTemporary()
        {
            return _currentStateElapsedTime > 0f;
        }
        private bool IsActionAllowed(CharacterAction characterAction)
        {
            if(RemainingDurationCharacterActionUnallowed(characterAction) > 0)
            {
                return false;
            }

            return true;
        }
        private float RemainingDurationCharacterActionUnallowed(CharacterAction characterAction)
        {
            var duration = 0f;
            _unallowedCharacterActions.TryGetValue(characterAction, out duration);
            return duration;
        }
        private void ProcessCurrentCharacterStateElapsedTime()
        {
            if(_currentStateElapsedTime > 500f)
            {
                return;
            }
            _currentStateElapsedTime += Time.deltaTime;
        }
        private void ResetCurrentCharacterStateElapsedTime()
        {
            _currentStateElapsedTime = 0f;
        }
      
        private void ResetStateDurations()
        {
            for(int i = 0; i<_currentStateDurations.Length; i++)
            {
                _currentStateDurations[i] = 0f;
            }
        }
        //private void UpdateStateDurationsBasedOnCharacterState(CharacterState)
        private void ReturnToInitialCharacterState(float delay)
        {
            StartCoroutine(ReturnToDefaultStateCO(delay));
        }
        private void ChangeCharacterStateAndTriggerEvent(CharacterState newCharacterState, float delay = 0f)
        {
            var blendDuration = GetActualDelayDurationOfTransition(newCharacterState);

            _currentCharacterState = newCharacterState;
            //InvokeCharacterStateChangedEvent(newCharacterState, delay);
            var data = _characterAnimationController.GetCharacterAnimationDataFromTemplate(newCharacterState);
            
            //FIX BLEND DURATION HERE
            Debug.Log("Animation change requested. New CharacterState  :    " + newCharacterState + " And blendDuration is  :  " + blendDuration);
            RequestAnimationChange(data, blendDuration);
        }
        private bool IsImmediateCharacterStateTransitionPossible(CharacterState newCharacterState)
        {
            if(_currentCharacterState.TransitionImmediatly.Contains(newCharacterState) == true)
            {
                return true;
            }

            return false;
        }

        private bool IsCharacterStateTransitionPossibleAfterFixedDelay(CharacterState newCharacterState, out float delay)
        {
            delay = 0f;
            var state = _currentCharacterState.TransitionAfterFixedDelay.Find(x => x.State == newCharacterState);
            if(state == null)
            {
                return false;
            } else
            {
                delay = state.Delay;
                return true;
            }
        }

        private bool IsCharacterStateTransitionPossibleAfterPercentageOfAnimationDelay(CharacterState newCharacterState, out float delay)
        {
            delay = 0f;
            var state = _currentCharacterState.TransitionAfterNormalizedTimeOfAnimationElapsed.Find(x => x.State == newCharacterState);
            if (state == null)
            {
                return false;
            }
            else
            {
                delay = state.Delay;
                return true;
            }
        }

        private bool IsCharacterStateTransitionPossibleIfForced(CharacterState newCharacterState)
        {
            return _currentCharacterState.TransitionAfterForcedStateChange.Contains(newCharacterState);
        }
        private void InvokeCharacterStateChangedEvent(CharacterState newCharacterState, float delay = 0f)
        {
            if (OnCharacterStateChanged != null)
            {
                OnCharacterStateChanged.Invoke(newCharacterState, delay);
            }
        }

        private void RequestAnimationChange(CharacterAnimationData animationData, float blendDuration)
        {
            if(_characterAnimationController == null || animationData == null || animationData.Animation == null)
            {
                return;
            }

            _characterAnimationController.PlayCharacterAnimationFromCharacterAnimationData(animationData, blendDuration);
        }

        private IEnumerator QueueNextCharacterStateCO(CharacterState newCharacterState, float delay)
        {
            var wait = new WaitForSeconds(Mathf.Max(0, delay));
            yield return wait;
            //TEST
            ChangeCharacterStateAndTriggerEvent(newCharacterState);
        }

        IEnumerator ReturnToDefaultStateCO(float delay)
        {
            yield return new WaitForSeconds(Mathf.Max(delay, 0f));
            //_currentCharacterState = CharacterStateEnum.Idle;
        }

        
    }
}