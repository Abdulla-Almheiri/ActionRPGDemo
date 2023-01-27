using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Systems;
using UnityEngine.SceneManagement;

namespace Chaos.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public AudioSource HeartBeatAudioSource;

        public GameAudioController GameAudioController;
        private CharacterAnimationController _characterAnimationController;
        private CharacterUIController _characterUIController;
        public CharacterCombatController PlayerCombatController { private set; get; }

        private float _audioEffectsAdjustRecharge = 0.2f;
        private float _audioEffectsAdjustCounter = 0.2f;

        public void Update()
        {
            ProcessSoundEffectsAdjustRecharge();
            AdjustSoundEffectsBasedOnHealthPercentage(PlayerCombatController.GetHealthPercentage());
            ProcessHeartBeatSoundEffect();
        }

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _characterUIController = GetComponent<CharacterUIController>();
            _characterUIController.ToggleHealthBar(false);
            _characterAnimationController = GetComponent<CharacterAnimationController>();

            if(GameAudioController == null)
            {
                GameAudioController = FindObjectOfType<GameAudioController>();
            }

            PlayerCombatController = GetComponent<CharacterCombatController>();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void AdjustSoundEffectsBasedOnHealthPercentage(float value)
        {
            if(IsSoundEffectsAdjustReady() == false)
            {
                //Debug.Log("Sound Effect adjust not ready  :  " + _audioEffectsAdjustCounter);
                return;
            }

            GameAudioController.SetLowPassFilterCutOffByPercentage(value);
            _audioEffectsAdjustCounter = _audioEffectsAdjustRecharge;
        }

        private bool IsSoundEffectsAdjustReady()
        {
            return _audioEffectsAdjustCounter <= 0f;
        }
        private void ProcessSoundEffectsAdjustRecharge()
        {
            if (_audioEffectsAdjustCounter > 0f)
            {
                _audioEffectsAdjustCounter -= Time.deltaTime;
            }
        }

        private void ProcessHeartBeatSoundEffect()
        {
            if (PlayerCombatController.Alive == true)
            {
                HeartBeatAudioSource.volume = Mathf.Lerp(1f, 0f, PlayerCombatController.GetHealthPercentage() / 100f + 0.6f);
            } else
            {
                HeartBeatAudioSource.volume = 0f;
            }
        }
    }
}
