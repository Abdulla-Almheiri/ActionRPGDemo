using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    [System.Serializable]
    public class CharacterAudioTemplateData 
    {
        public List<AudioClip> AudioClips = new List<AudioClip>();
        [Range(0f, 1f)]
        public float BaseVolume = 0.5f;
        public float PitchVariation = 10f;
        public float VolumeVariation = 10f;
    }
}