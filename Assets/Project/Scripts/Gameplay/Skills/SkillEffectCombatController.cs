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

        private bool _hitBoxActive = false;
        private bool _attackPerformed = false;
        private float _delay = 0f;
        private bool _selfSkill = false;

        private void Update()
        {
            ProcessDelay();
        }
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
        private void OnTriggerStay(Collider other)
        {
            if(_hitBoxActive == false)
            {
                return;
            }

            
            var otherCombatController = other.gameObject.GetComponent<CharacterCombatController>();
            if(otherCombatController == _skillActivator && !_selfSkill)
            {
                return;
            }

            TriggerHitOnCharacter(otherCombatController);
        }

        public void Initialize(SkillTemplate skillTemplate, CharacterCombatController activator, float delay, bool isSelfSkill)
        {
            SkillTemplate = skillTemplate;
           _skillActions = skillTemplate.SkillActions;
            _skillActivator = activator;
            _delay = delay;
            _selfSkill = isSelfSkill;
        }

        public void ActivateHitBox()
        {
            _hitBoxActive = true;
        }


        private void ProcessDelay()
        {
            _delay -= Time.deltaTime;
            if(_delay <= 0f)
            {
                ActivateHitBox();
            }
        }

    }
}