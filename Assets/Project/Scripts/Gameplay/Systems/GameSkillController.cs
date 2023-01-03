using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Systems
{
    public class GameSkillController : MonoBehaviour
    {
        public GameObjectPoolTemplate SkillGamePoolTest;
        private GameObjectPool _skillObjectPool;
        private GamePoolController _gamePoolController;
        private Dictionary<string, SkillEffectCombatController> _pooledSkills = new Dictionary<string, SkillEffectCombatController>();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Initialize()
        {
            _gamePoolController = GetComponent<GamePoolController>();
            _skillObjectPool = new GameObjectPool(SkillGamePoolTest, _gamePoolController);
        }

        public void SpawnSkillEffectPooled(SkillTemplate skillTemplate, Transform location, CharacterCombatController activator)
        {
            SkillEffectCombatController skillCombatController;
            if (_pooledSkills.TryGetValue(skillTemplate.name, out skillCombatController) == true && skillCombatController.gameObject.activeSelf == false)
            {
                skillCombatController.transform.position = location.position;
                skillCombatController.Initialize(skillTemplate, activator);
                skillCombatController.gameObject.SetActive(true);
                Debug.Log("First if statement. Object is  :  " + location.gameObject.name);

            } else
            {
                var instance = InstantiateSkillEffect(skillTemplate, location, activator);
                AddSkillInstanceToPool(skillTemplate, instance);
            }


        }

        private SkillEffectCombatController InstantiateSkillEffect(SkillTemplate skillTemplate, Transform location, CharacterCombatController activator)
        {
            var spawn = Instantiate(skillTemplate.SkillPrefab, location);
            spawn.transform.SetParent(null);

            var skillCombatController = spawn.GetComponent<SkillEffectCombatController>();

            if (skillCombatController == null)
            {
                return null;
            }

            
            skillCombatController.Initialize(skillTemplate, activator);
            return skillCombatController;
        }


        private void AddSkillInstanceToPool(SkillTemplate skillTemplate, SkillEffectCombatController skillInstance)
        {
            _pooledSkills[skillTemplate.name] = skillInstance;
        }
    }
}