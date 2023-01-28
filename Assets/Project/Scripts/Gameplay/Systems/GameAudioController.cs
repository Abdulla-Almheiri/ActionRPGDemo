using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.UI;
using Chaos.Gameplay.Player;
using UnityEngine.Audio;

namespace Chaos.Gameplay.Systems {
    public class GameAudioController : MonoBehaviour
    {
        public GameAudioTemplate GameAudioTemplate;
        public float MasterVolume { private set; get; } = -10f;
        public float SoundEffectsVolume { private set; get; } = -10f;
        public float MusicVolume { private set; get; } = -10f;

        private float _lowPassFilterCutOff = 22000f;

        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";


        private PlayerController _playerController;
        public AudioSource AudioSourceComponent { private set; get; }
        private void Awake()
        {
            AudioSourceComponent = GetComponent<AudioSource>();
            InitializePlayerPrefs();
        }

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            RefreshMixer();
  
        }
        private void Update()
        {
            ProcessLowPassFilter();
        }

        private void RefreshMixer()
        {
            SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOLUME_KEY));
            SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY));
            SetSoundEffectsVolume(PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME_KEY));
        }
        public void SetMasterVolume(float value)
        {
            MasterVolume = value;
            GameAudioTemplate.AudioMixer.SetFloat("MasterVolume", MasterVolume);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
        }

        public void SetSoundEffectsVolume(float value)
        {
            SoundEffectsVolume = value;
            GameAudioTemplate.AudioMixer.SetFloat("SoundEffectsVolume", SoundEffectsVolume);
            PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, SoundEffectsVolume);
        }

        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            GameAudioTemplate.AudioMixer.SetFloat("MusicVolume", MusicVolume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
            Debug.Log("Music Volume PlayerPrefs  :  " + MusicVolume);
        }

        private void InitializePlayerPrefs()
        {
            if (PlayerPrefs.HasKey(MASTER_VOLUME_KEY) == true)
            {
                SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOLUME_KEY));
            }
            else
            {
                PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
            }

            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY) == true)
            {
                SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY));
            }
            else
            {
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
            }

            if (PlayerPrefs.HasKey(SOUND_EFFECTS_VOLUME_KEY) == true)
            {
                SetSoundEffectsVolume(PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME_KEY));
            }
            else
            {
                PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, SoundEffectsVolume);
            }

        }

        public void PlayMessageSound(PlayerUIMessage message)
        {
            //FIX HERE. Make it affected by soundeffect volume
            if(message.SoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(message.SoundEffect, _playerController.gameObject.transform.position, GetSoundEffectsVolumeNormalized()*message.VolumeOverride);
            }

            Debug.Log("Normalized sound effects volume =   " + GetSoundEffectsVolumeNormalized());
        }

        public float GetSoundEffectsVolumeNormalized()
        {
            return Mathf.Max(0 , (SoundEffectsVolume + 40f) / 40f);
        }

        private void SetSoundEffectsLowPassFilterCutOff(float value)
        {
            _lowPassFilterCutOff = Mathf.Clamp(value, 700f, 22000f);
        }

        private void ProcessLowPassFilter()
        {
            GameAudioTemplate.AudioMixer.SetFloat("LowPassFilterCutOff", _lowPassFilterCutOff);
        }

        public void SetLowPassFilterCutOffByPercentage(float value)
        {
            _lowPassFilterCutOff = Mathf.Lerp(700f, 22000f, value/100f);
        }

        public void PlayUIConfirmSound()
        {
            //AudioSource.PlayClipAtPoint(GameAudioTemplate.UIConfirmSound, gameObject.transform.position, GetSoundEffectsVolumeNormalized());
            PlaySound(GameAudioTemplate.UIConfirmSound);
        }

        public void PlayUICancelSound()
        {
            // AudioSource.PlayClipAtPoint(GameAudioTemplate.UICancelSound, gameObject.transform.position, GetSoundEffectsVolumeNormalized());
            PlaySound(GameAudioTemplate.UICancelSound);
        }

        public void PlayUIWindowOpenSound()
        {
            // AudioSource.PlayClipAtPoint(GameAudioTemplate.UIWindowOpenSound, gameObject.transform.position, GetSoundEffectsVolumeNormalized());
            PlaySound(GameAudioTemplate.UIWindowOpenSound);
        }

        public void PlayUIHoverSound()
        {
            // AudioSource.PlayClipAtPoint(GameAudioTemplate.UIHoverSound, gameObject.transform.position, GetSoundEffectsVolumeNormalized());
            PlaySound(GameAudioTemplate.UIHoverSound);
        }

        public void PlaySound(AudioClip clip)
        {
            AudioSourceComponent.PlayOneShot(clip);
        }
    }
}