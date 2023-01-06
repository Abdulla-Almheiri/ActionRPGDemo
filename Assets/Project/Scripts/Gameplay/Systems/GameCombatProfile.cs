using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "new game combat profile", menuName = "Content/Game/Game Combat Profile")]
    public class GameCombatProfile : ScriptableObject
    {
        public List<CharacterAttributeRatingToPercentageConversionData> CharacterAttributeRatingConverstionDetails = new List<CharacterAttributeRatingToPercentageConversionData>();
    }
}