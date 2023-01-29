using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.UI;
using Chaos.Gameplay.Player;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Systems
{
    public class TriggerUIMessageController : MonoBehaviour
    {
        public PlayerUIMessage Message;
        public float Duration = 3f;
        public UIMessageTriggerType TriggerType = UIMessageTriggerType.CollideWithTrigger;
        private GameUIController _gameUIController;
        private CharacterCombatController _characterCombatController;
        private bool _triggered = false;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _gameUIController = FindObjectOfType<GameUIController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
        }

        private void Update()
        {
            ProcessDeathMessage();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_triggered == false && Message != null && TriggerType == UIMessageTriggerType.CollideWithTrigger)
            {
                if(other.GetComponent<PlayerController>() != null)
                {
                    TriggerAndDestroy();
                }
            }
        }

        private void TriggerAndDestroy()
        {
            TriggerAndStay();
            Destroy(gameObject, 1f);
        }

        private void TriggerAndStay()
        {
            if(Message == null)
            {
                return;
            }

            _gameUIController?.TriggerPlayerUIMessage(Message, Duration);
            _triggered = true;
        }

        private void ProcessDeathMessage()
        {
            if(_characterCombatController == null || _triggered == true)
            {
                return;
            }

            if(TriggerType == UIMessageTriggerType.OnCharacterDeath && _characterCombatController.Alive == false)
            {
                TriggerAndStay();
            }

        }
    }

    public enum UIMessageTriggerType
    {
        CollideWithTrigger,
        OnCharacterDeath
    }
}