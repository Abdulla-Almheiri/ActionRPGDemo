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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void TriggerHitOnCharacter(CharacterCombatController character)
        {
            if(character == null)
            {
                return;
            }

            character.TriggerHitBySkillEffect(this, _skillActivator);
            if (_skillActions == null)
            {
                Debug.Log("List is null");
            }
            foreach (SkillAction skillAction in _skillActions)
            {
                character.ApplySkillAction(skillAction, _skillActivator);
            }

            character.TriggerHitBySkillEffect(this, _skillActivator);

        }
        private void OnTriggerEnter(Collider other)
        {
            var otherCombatController = other.gameObject.GetComponent<CharacterCombatController>();
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