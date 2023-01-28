using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [CreateAssetMenu(fileName = "new character audio template", menuName = "Content/Characters/Character Audio Template")]
    public class CharacterAudioTemplate : ScriptableObject
    {
        public CharacterAudioTemplateData Engage;
        public CharacterAudioTemplateData Footsteps;
        public CharacterAudioTemplateData Death;
        public CharacterAudioTemplateData Hit;
        public CharacterAudioTemplateData BasicAttack;
        public CharacterAudioTemplateData SpecialAttack;


    }
}