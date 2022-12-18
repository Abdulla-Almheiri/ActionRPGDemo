using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterStateController : MonoBehaviour
    {
        protected CharacterStateEnum _currentCharacterState = CharacterStateEnum.Idle;

        protected void ReturnToDefaultState(float delay)
        {
            StartCoroutine(ReturnToDefaultStateCO(delay));
        }

        IEnumerator ReturnToDefaultStateCO(float delay)
        {
            yield return new WaitForSeconds(Mathf.Max(delay, 0f));
            _currentCharacterState = CharacterStateEnum.Idle;
        }


        public CharacterStateEnum GetCurrentState()
        {
            return _currentCharacterState;
        }

        public void SetCurrentState(CharacterStateEnum newCharacterState)
        {
            
        }
    }

    public enum CharacterStateEnum
    {
        LightCasting,
        HeavyCasting,
        Moving,
        Idle,

    }
}