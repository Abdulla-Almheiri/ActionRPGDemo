using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character state", menuName = "Content/Characters/Character State")]
    public class CharacterState : ScriptableObject
    {
        /*public bool CanMove = true;
        public bool CanBasicAttack = true;
        public bool CanSpecialAttack = true;
        public bool CanDefend = true;
        public bool Damageable = true;
        public bool Healable = true;
        public bool Highlightable = true;*/
        public string AnimationName = "";
        public bool Looping = false;
        public CharacterState NextCharacterStateAfterAnimationFinished;
        public List<CharacterState> TransitionImmediatly = new List<CharacterState>();
        public List<CharacterStateData> TransitionAfterFixedDelay = new List<CharacterStateData>();
        public List<CharacterStateData> TransitionAfterDelayPercentageOfCurrentAnimation = new List<CharacterStateData>();
        public List<CharacterState> TransitionAfterForcedStateChange = new List<CharacterState>();
        public List<CharacterState> AlwaysIgnore = new List<CharacterState>();

        /*public CharacterState(bool canMove, bool canBasicAttack, bool canSpecialAttack, bool canDefend, bool damageable, bool healable)
        {
            CanMove = canMove;
            CanBasicAttack = canBasicAttack;
            CanSpecialAttack = canSpecialAttack;
            CanDefend = canDefend;
            Damageable = damageable;
            Healable = healable;
        }*/
    }

    public enum CharacterStateDataEnum
    {
        AlwaysInterrupt,
        InterruptAfterFixedDelay,
        InterruptAfterPercentageOfCurrentAnimation,
        Uninterruptible
    }
}