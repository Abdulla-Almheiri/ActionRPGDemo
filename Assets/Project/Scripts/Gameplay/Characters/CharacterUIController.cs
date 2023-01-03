using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.UI;

namespace Chaos.Gameplay.Characters
{
    public class CharacterUIController : MonoBehaviour
    {
        [SerializeField]
        private Transform _healthBarLocation;
        public GameUIController GameUIController;
        public GameUIController GameUIControllerTest;
        private HealthBarPrefabController _healthBarPrefabInstance;

        private CharacterCombatController _characterCombatController;
        // Start is called before the first frame update
        void Start()
        {
            ToggleHealthBar(true);
            Initialize();
        }


        private void Initialize()
        {
            _characterCombatController = GetComponent<CharacterCombatController>();
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void ToggleHealthBar(bool state)
        {
            if(state == true)
            {
                if(_healthBarPrefabInstance == null)
                {
                    _healthBarPrefabInstance = InstantiateHealthBarPrefab(_healthBarLocation);
                } else
                {
                    _healthBarPrefabInstance.gameObject.SetActive(true);
                    _healthBarPrefabInstance.SetHealthPercentage(_characterCombatController.GetHealthPercentage());
                }
            } else
            {
                _healthBarPrefabInstance?.gameObject.SetActive(state);
            }
        }

        private HealthBarPrefabController InstantiateHealthBarPrefab(Transform location)
        {
            var spawn = Instantiate(GameUIController.HealthBarPrefab, GameUIController.Canvas.transform);
            spawn.Initialize(location);
            return spawn;
        }

        public void UpdateHealth()
        {
            _healthBarPrefabInstance.SetHealthPercentage(_characterCombatController.GetHealthPercentage());
        }
    }
}