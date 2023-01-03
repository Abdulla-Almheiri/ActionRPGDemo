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
        public Canvas Canvas;
        public HealthBarPrefabController HealthBarPrefab;
        public FloatingCombatTextProfile FloatingCombatTextProfile;
        public FloatingCombatTextTemplate FloatingCombatTextTemplate;
        public GameObjectPoolTemplate GameObjectPoolTemplate;
        private GamePoolController _gamePoolController;

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
            
        }

        public void SpawnDamageTextAtScreenPositionTest(string text, Transform worldPoint)
        {
            var objPool = _gamePoolController.RetrieveGameObjectPoolByTemplate(GameObjectPoolTemplate);
            var spawn = objPool.RetrieveNextAvailableGameObjectFromPool();
            if(spawn == null)
            {
                
                return;
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
        CriticalDamage,
        Healing,
        CriticalHealing,
        CharacterStatusChange,
        HealingRegeneration,
        EnergyRegeneration

    }
}
