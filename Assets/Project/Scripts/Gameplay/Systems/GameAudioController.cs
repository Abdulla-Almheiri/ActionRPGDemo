using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.UI;
using Chaos.Gameplay.Player;


namespace Chaos.Gameplay.Systems {
    public class GameAudioController : MonoBehaviour
    {
        
        public float MusicVolume { private set; get; } = 0.5f;
        public float SFXVolume { private set; get; } = 0.5f;
        private const string SFX_VOLUME_KEY = "SFX";
        private const string MUSIC_VOLUME_KEY = "Music";

        private PlayerController _playerController;
        private void Awake()
        {
            InitializePlayerPrefs();
        }

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }
        public void SetSFXVolume(float value)
        {
            /*if (value < 0f || value > 1f)
            {
                return;
            }*/

            AudioListener.volume = value;
            SFXVolume = value;
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
        }

        public void SetMusicVolume(float value)
        {
            /* if(value < 0f || value > 1f)
             {
                 return;
             }*/

            MusicVolume = value;
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
        }

        private void InitializePlayerPrefs()
        {
            if (PlayerPrefs.HasKey(SFX_VOLUME_KEY) == true)
            {
                SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
                Debug.Log("SFX Volume obtained from PlayerPrefs :    " + SFXVolume);
            }
            else
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

        public void PlayMessageSound(PlayerUIMessage message)
        {
            if(message.SoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(message.SoundEffect, _playerController.gameObject.transform.position, 0.5f);
            }
        }
    }
}