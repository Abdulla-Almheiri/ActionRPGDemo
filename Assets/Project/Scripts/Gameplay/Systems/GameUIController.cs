using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using TMPro;
using Chaos.Gameplay.UI;
using UnityEngine.UI;

namespace Chaos.Gameplay.Systems
{
    public class GameUIController : MonoBehaviour
    {
        public HUDController HUDController;
        public Canvas Canvas;
        public HealthBarPrefabController HealthBarPrefab;
        public FloatingCombatTextProfile FloatingCombatTextProfile;
        public FloatingCombatTextTemplate FloatingCombatTextTemplate;
        public GameObjectPoolTemplate GameObjectPoolTemplate;
        private GamePoolController _gamePoolController;
        private GameAudioController _gameAudioController;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initialize()
        {
            _gamePoolController = GetComponent<GamePoolController>();
            _gameAudioController = GetComponent<GameAudioController>();
            
        }

        public void ShowRestartLevelMenu()
        {
            HUDController.ShowRestartLevelMenu();
        }
        public void TriggerPlayerUIMessage(PlayerUIMessage message, float duration)
        {
            HUDController.TriggerPlayerUIMessage(message, duration);
            _gameAudioController.PlayMessageSound(message);

            Debug.Log("Message triggered :  " + message.Text);

        }
        public void SpawnDamageTextAtScreenPositionTest(string text, Transform worldPoint, FloatingCombatTextEventType combatEventType)
        {
            //FIX HERE
            var objPool = _gamePoolController.RetrieveGameObjectPoolByTemplate(GameObjectPoolTemplate);
            var spawn = objPool.RetrieveNextAvailableGameObjectFromPool();
            if(spawn == null)
            {
                Debug.Log("Spawn is null");
                return;
            }

            switch(combatEventType)
            {
                case FloatingCombatTextEventType.Damage:
                    FloatingCombatTextTemplate = FloatingCombatTextProfile.Damage;
                    break;
                case FloatingCombatTextEventType.DamageTaken:
                    FloatingCombatTextTemplate = FloatingCombatTextProfile.DamageTakenAsPlayer;
                    break;
                case FloatingCombatTextEventType.CriticalDamage:
                    FloatingCombatTextTemplate = FloatingCombatTextProfile.CriticalDamage;
                    break;
                case FloatingCombatTextEventType.Healing:
                    FloatingCombatTextTemplate = FloatingCombatTextProfile.Healing;
                    break;
                case FloatingCombatTextEventType.CriticalHealing:
                    FloatingCombatTextTemplate = FloatingCombatTextProfile.CriticalHealing;
                    break;
                default:
                    break;
            }

            spawn.gameObject.SetActive(true);
            spawn.gameObject.GetComponent<FloatingCombatTextAnimationController>().Initialize(worldPoint, FloatingCombatTextTemplate);
            spawn.gameObject.GetComponent<FloatingCombatTextAnimationController>().Play(worldPoint, FloatingCombatTextTemplate, true);
            //spawn.gameObject.GetComponent<TMP_Text>().rectTransform.SetPositionAndRotation(targetScreenPosition, Quaternion.identity);
            spawn.gameObject.GetComponent<TMP_Text>().text = text;
            spawn.gameObject.GetComponent<TMP_Text>().faceColor = FloatingCombatTextTemplate.Color;


        }

        public void TriggerFloatingCombatTextFromTransform(string text, Transform worldPoint, FloatingCombatTextEventType eventType)
        {

        }
    }

    public enum FloatingCombatTextEventType
    {
        Damage,
        DamageTaken,
        CriticalDamage,
        Healing,
        CriticalHealing,
        CharacterStatusChange,
        HealingRegeneration,
        EnergyRegeneration

    }
}
