using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character state", menuName = "Content/Characters/Character State")]
    public class CharacterState : ScriptableObject
    {
        public CharacterAnimation CharacterAnimation;
        public bool IsAnimationLooping = false;
        public CharacterAction TriggeredBy;
        [Tooltip("Delay is normalized elapsed time of current animation.")]
        public CharacterState AutomaticStateTransitionData;
        public CharacterConditionData CausedCondition;
        public List<CharacterStateTransitionAdditionalData> Transitions = new List<CharacterStateTransitionAdditionalData>();

        public List<CharacterActionData> Actions = new List<CharacterActionData>();
       /* [Space(10)]
        [Header("Allowed Actions")]
        public List<CharacterAction> CharacterActions = new List<CharacterAction>();

        [Space(10)]
        [Header("Allowed Transitions")]
        public List<CharacterState> TransitionImmediatly = new List<CharacterState>();
        public List<CharacterStateTransitionData> TransitionAfterFixedDelay = new List<CharacterStateTransitionData>();
        public List<CharacterStateTransitionData> TransitionAfterNormalizedTimeOfAnimationElapsed = new List<CharacterStateTransitionData>();
        public List<CharacterState> TransitionAfterForcedStateChange = new List<CharacterState>();
        [Space(10)]
        [Header("No Transitions Possible")]
        public List<CharacterState> AlwaysIgnore = new List<CharacterState>();*/
    }
}