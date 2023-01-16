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
        public SkillTemplate SkillTemplate { private set; get; }

        private List<SkillAction> _skillActions;
        private CharacterCombatController _skillActivator;
        private Dictionary<CharacterCombatController, bool> _charactersAlreadyHit = new Dictionary<CharacterCombatController, bool>();
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

            if (_skillActions == null)
            {
                
            }

            if(_charactersAlreadyHit.TryGetValue(character, out bool _) == true)
            {
                return;
            }

            foreach (SkillAction skillAction in _skillActions)
            {
                character.TriggerHitBySkillAction( _skillActivator, SkillTemplate, skillAction);
            }
            
            character.TriggerHitBySkillEffect(this, _skillActivator);

            _charactersAlreadyHit.TryAdd(character, true);

        }
        private void OnTriggerEnter(Collider other)
        {
            var otherCombatController = other.gameObject.GetComponent<CharacterCombatController>();
            if(otherCombatController == _skillActivator)
            {
                return;
            }

            TriggerHitOnCharacter(otherCombatController);
        }

        public void Initialize(SkillTemplate skillTemplate, CharacterCombatController activator)
        {
            SkillTemplate = skillTemplate;
           _skillActions = skillTemplate.SkillActions;
            _skillActivator = activator;
        }


    }
}