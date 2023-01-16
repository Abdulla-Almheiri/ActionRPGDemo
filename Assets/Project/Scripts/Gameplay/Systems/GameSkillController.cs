using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Systems
{
    public class GameSkillController : MonoBehaviour
    {
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Initialize()
        {
        }

        public bool SpawnSkillVFX(SkillTemplate skillTemplate, Transform location, CharacterCombatController activator)
        {
            if (skillTemplate == null || skillTemplate.SkillPrefab == null)
            {
                return false;
            }

            var instance = InstantiateSkillEffect(skillTemplate, location, activator);
            return true;
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
    }
}