using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.Player;

namespace Chaos.Gameplay.Skills
{
    public class SkillEffectCombatController : MonoBehaviour
    {
        public SkillActionElement ElementTest;
        public GameUIController GameUIControllerTest;

        public SkillTemplate SkillTemplate { private set; get; }

        private List<SkillAction> _skillActions;
        private CharacterCombatController _skillActivator;
        private void TriggerHitOnCharacter(CharacterCombatController character)
        {


            if (character == null)
            {
                return;
            }

            if (character.Alive == false)
            {
                return;
            }

            // character.TriggerHitBySkillEffect(this, _skillActivator);
            if (_skillActions == null)
            {
                
            }
            foreach (SkillAction skillAction in _skillActions)
            {
                character.TriggerHitBySkillAction( _skillActivator, SkillTemplate, skillAction);
            }
            
            character.TriggerHitBySkillEffect(this, _skillActivator);

        }
        private void OnTriggerEnter(Collider other)
        {
            var otherCombatController = other.gameObject.GetComponent<CharacterCombatController>();
            if(otherCombatController.gameObject.GetComponent<PlayerController>() == true)
            {
                return;
            }
            TriggerHitOnCharacter(otherCombatController);
        }

        public void Initialize(SkillTemplate skillTemplate, CharacterCombatController activator)
        {
            SkillTemplate = skillTemplate;
            //TEST
           _skillActions = skillTemplate.SkillActions;
            _skillActivator = activator;
        }


    }
}