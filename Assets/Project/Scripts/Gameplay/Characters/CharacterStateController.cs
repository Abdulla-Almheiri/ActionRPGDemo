using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private Queue<CharacterStateData> _queuedCharacterStateChangeCommands = new Queue<CharacterStateData>();

        private float[] _currentStateDurations;

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

        private void Initialize(CharacterState initialCharacterState)
        {
            _currentCharacterState = initialCharacterState;
            _initialCharacterstate = initialCharacterState;

            _currentStateDurations = new float[5];
            ResetStateDurations();

            //_currentState = new CharacterState(true, true, true, true, true, true);
        }

        private void ResetStateDurations()
        {
            for(int i = 0; i<_currentStateDurations.Length; i++)
            {
                _currentStateDurations[i] = 0f;
            }
        }

        //private void UpdateStateDurationsBasedOnCharacterState(CharacterState)
        protected void ReturnToInitialCharacterState(float delay)
        {
            StartCoroutine(ReturnToDefaultStateCO(delay));
        }

        IEnumerator ReturnToDefaultStateCO(float delay)
        {
            yield return new WaitForSeconds(Mathf.Max(delay, 0f));
            //_currentCharacterState = CharacterStateEnum.Idle;
        }


        public CharacterState GetCurrentState()
        {
            return _currentCharacterState;
        }

        public bool SetCurrentCharacterStateIfTransitionIsImmediateOnly(CharacterState newCharacterState)
        {
            if(IsImmediateCharacterStateTransitionPossible(newCharacterState) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState);
            }

            ResetCurrentCharacterStateElapsedTime();
            return true;
        }

        private void ChangeCharacterStateAndTriggerEvent(CharacterState newCharacterState, float delay = 0f)
        {
            _currentCharacterState = newCharacterState;
            InvokeCharacterStateChangedEvent(newCharacterState, delay);
        }
        public bool SetCurrentCharacterStateIfTransitionHasDelayOnly(CharacterState newCharacterState)
        {
            if (IsCharacterStateTransitionPossibleAfterFixedDelay(newCharacterState, out float delay) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState, delay);
                return true;
            } else if(IsCharacterStateTransitionPossibleAfterPercentageOfAnimationDelay(newCharacterState, out float animationDelay) == true)
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
            if(SetCurrentCharacterStateIfTransitionIsImmediateOnly(newCharacterState) == true)
            {
                return true;
            } else if(SetCurrentCharacterStateIfTransitionHasDelayOnly(newCharacterState) == true)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool SetCurrentCharacterStateTransitionForcedOnly(CharacterState newCharacterState)
        {
            if(IsCharacterStateTransitionPossibleIfForced(newCharacterState) == true)
            {
                ChangeCharacterStateAndTriggerEvent(newCharacterState);
                return true;
            } else
            {
                return false;
            }
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
            var state = _currentCharacterState.TransitionAfterDelayPercentageOfCurrentAnimation.Find(x => x.State == newCharacterState);
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

        public void QueueNextCharacterState(CharacterState newCharacterState, float delay)
        {
            /*if(SetCurrentCharacterStateIfTransitionIsImmediateOnly(newCharacterState) == true)
            {
                return;
            }*/
            StopAllCoroutines();
            StartCoroutine(QueueNextCharacterStateCO(newCharacterState, delay));
        }

        private IEnumerator QueueNextCharacterStateCO(CharacterState newCharacterState, float delay)
        {
            var wait = new WaitForSeconds(Mathf.Max(0, delay));
            yield return wait;
            //TEST
            ChangeCharacterStateAndTriggerEvent(newCharacterState);
        }

        public void SubscribeToCharacterStateChanged(CharacterStateChanged subscriber)
        {
            OnCharacterStateChanged += subscriber;
        }

        public void UnsubscribeToCharacterStateChanged(CharacterStateChanged unsubscriber)
        {
            OnCharacterStateChanged -= unsubscriber;
        }

        private void InvokeCharacterStateChangedEvent(CharacterState newCharacterState, float delay = 0f)
        {
            if(OnCharacterStateChanged != null)
            {
                OnCharacterStateChanged.Invoke(newCharacterState, delay);
            }
        }
    }

    public enum CharacterStateEnum
    {
        Attacking_Basic,
        Attacking_Special,
        Defending,
        Walking,
        Running,
        Idle,
        Stunned,
        Rooted,
        Dead

    }
}