using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterAudioController : MonoBehaviour
    {
        [field: SerializeField]
        public AudioSource AudioSource { private set; get; }
        [field: SerializeField]
        public AudioSource AudioSourceFootsteps { private set; get; }

        [field: SerializeField]
        public CharacterAudioTemplate CharacterAudioTemplate { private set; get; }

        private CharacterMovementController _characterMovementController;
        private CharacterCombatController _characterCombatController;
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            ProcessFootstepsSound();
        }

        private void Initialize()
        {
            if (AudioSource == null)
            {
                AudioSource = GetComponent<AudioSource>();
            }

            _characterMovementController = GetComponent<CharacterMovementController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
        }

        private void ProcessFootstepsSound()
        {
            if(CharacterAudioTemplate == null)
            {
                return;
            }

            if(AudioSourceFootsteps == null)
            {
                return;
            }

            if (_characterCombatController.Alive == false)
            {
                AudioSourceFootsteps.volume = 0f;
                return;
            }

            float volumeMultiplier = _characterMovementController.GetVelocityPercentageOfMaximumSpeed();

            if (AudioSourceFootsteps.isPlaying == false)
            {
                if (CharacterAudioTemplate.Footsteps.AudioClips == null)
                {
                    return;
                }

                var index = Random.Range(0, CharacterAudioTemplate.Footsteps.AudioClips.Count - 1);
                if(index < 0)
                {
                    return;
                }

                if(CharacterAudioTemplate.Footsteps.AudioClips.Count == 0)
                {
                    return;
                }

                var clip = CharacterAudioTemplate.Footsteps.AudioClips[Random.Range(0, index)];
                if (clip == null)
                {
                    return;
                }

                var volume = CharacterAudioTemplate.Footsteps.BaseVolume + Random.Range(-1 * CharacterAudioTemplate.Footsteps.VolumeVariation, CharacterAudioTemplate.Footsteps.VolumeVariation) / 100f;

                var pitch = 1f;
                pitch = pitch + (pitch * Random.Range(-1 * CharacterAudioTemplate.Footsteps.PitchVariation, CharacterAudioTemplate.Footsteps.PitchVariation) / 100f);
                AudioSourceFootsteps.volume = volume * volumeMultiplier;
                AudioSourceFootsteps.pitch = pitch;
                AudioSourceFootsteps.PlayOneShot(clip);

            }
        }

        public void PlayDeathSound()
        {
            if (CharacterAudioTemplate == null)
            {
                return;
            }

            if (CharacterAudioTemplate.Death.AudioClips == null)
            {
                return;
            }
            var clip = GetRandomClipFromList(CharacterAudioTemplate.Death.AudioClips);
            if (clip == null)
            {
                return;
            }
            var volume = GetRandomVolumeFromCharacterAudioTemplateData(CharacterAudioTemplate.Death);
            PlaySoundOneShot(clip, volume);
        }

        public void PlayEngageSound()
        {
            if (CharacterAudioTemplate == null)
            {
                return;
            }

            if (CharacterAudioTemplate.Engage.AudioClips == null)
            {
                return;
            }
            var clip = GetRandomClipFromList(CharacterAudioTemplate.Engage.AudioClips);
            if (clip == null)
            {
                return;
            }
            var volume = GetRandomVolumeFromCharacterAudioTemplateData(CharacterAudioTemplate.Engage);
            PlaySoundOneShot(clip, volume);
        }

        public void PlayHitSound()
        {
            if (CharacterAudioTemplate == null)
            {
                return;
            }

            if (CharacterAudioTemplate.Hit.AudioClips == null)
            {
                return;
            }

            var clip = GetRandomClipFromList(CharacterAudioTemplate.Hit.AudioClips);
            if (clip == null)
            {
                return;
            }
            var volume = GetRandomVolumeFromCharacterAudioTemplateData(CharacterAudioTemplate.Hit);
            PlaySoundOneShot(clip, volume);
        }

        public void PlayBasicAttackSound()
        {
            if (CharacterAudioTemplate == null)
            {
                return;
            }

            if (CharacterAudioTemplate.BasicAttack.AudioClips == null)
            {
                return;
            }
            var clip = GetRandomClipFromList(CharacterAudioTemplate.BasicAttack.AudioClips);
            if (clip == null)
            {
                return;
            }

            var volume = GetRandomVolumeFromCharacterAudioTemplateData(CharacterAudioTemplate.BasicAttack);
   
            PlaySoundOneShot(clip, volume);
        }

        public void PlaySpecialAttackSound()
        {
            if(CharacterAudioTemplate == null)
            {
                return;
            }

            if (CharacterAudioTemplate.SpecialAttack.AudioClips == null)
            {
                return;
            }

            var clip = GetRandomClipFromList(CharacterAudioTemplate.SpecialAttack.AudioClips);
            if (clip == null)
            {
                return;
            }
            var volume = GetRandomVolumeFromCharacterAudioTemplateData(CharacterAudioTemplate.SpecialAttack);
            PlaySoundOneShot(clip, volume);
        }

        public void PlaySoundOneShot(AudioClip clip, float volume)
        {
            if(AudioSource == null)
            {
                return;
            }

            if (clip == null)
            {
                return;
            }
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }

        private AudioClip GetRandomClipFromList(List<AudioClip> list)
        {
            if(list.Count == 0)
            {
                return null;
            }
            return list[Random.Range(0, list.Count - 1)];
        }


        private float GetRandomVolumeFromCharacterAudioTemplateData(CharacterAudioTemplateData audioData)
        {
            var volume = audioData.BaseVolume;
            var volumeVariation = audioData.VolumeVariation;
            volume = volume + (volume * Random.Range(-1 * volumeVariation, volumeVariation)) / 100f;
            return volume;
        }
    }
}