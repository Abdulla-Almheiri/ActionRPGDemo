using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Chaos.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "new game audio template", menuName = "Content/Audio/Game Audio Template")]
    public class GameAudioTemplate : ScriptableObject
    {
        public AudioMixer AudioMixer;

        public AudioClip UIWindowOpenSound;
        public AudioClip UIConfirmSound;
        public AudioClip UICancelSound;
        public AudioClip UIHoverSound;

        public AudioMixerGroup MasterMixerGroup;
        public AudioMixerGroup SoundEffectsMixerGroup;
        public AudioMixerGroup MusicMixerGroup;

        public AudioMixerGroup GameSoundsMixerGroup;
        public AudioMixerGroup UISoundsMixerGroup;
        public AudioMixerGroup AmbientSoundsMixerGroup;


    }
}