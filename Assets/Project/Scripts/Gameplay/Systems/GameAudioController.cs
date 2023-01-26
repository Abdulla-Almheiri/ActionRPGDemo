using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Systems {
    public class GameAudioController : MonoBehaviour
    {
        public float MusicVolume { private set; get; } = 0.5f;
        public float SFXVolume { private set; get; } = 0.5f;
        private const string SFX_VOLUME_KEY = "SFX";
        private const string MUSIC_VOLUME_KEY = "Music";

        private void Awake()
        {
            InitializePlayerPrefs();
        }

        public void SetSFXVolume(float value)
        {
            if (value < 0f || value > 1f)
            {
                return;
            }

            AudioListener.volume = value;
            SFXVolume = value;
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
        }

        public void SetMusicVolume(float value)
        {
            if(value < 0f || value > 1f)
            {
                return;
            }

            MusicVolume = value;
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
        }

        private void InitializePlayerPrefs()
        {
            if(PlayerPrefs.HasKey(SFX_VOLUME_KEY) == true)
            {
                SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
            } else
            {
                PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
            }

            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY) == true)
            {
                MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
            }
            else
            {
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
            }
        }
    }
}