using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.UI;
using Chaos.Gameplay.Player;

namespace Chaos.Gameplay.Systems
{
    public class TriggerUIMessageController : MonoBehaviour
    {
        public PlayerUIMessage Message;
        public float Duration = 3f;
        private GameUIController _gameUIController;
        private bool _triggered = false;
        // Start is called before the first frame update
        void Start()
        {
            _gameUIController = FindObjectOfType<GameUIController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_triggered == false && Message != null)
            {
                if(other.GetComponent<PlayerController>() != null)
                {
                    _gameUIController?.TriggerPlayerUIMessage(Message, Duration);
                    _triggered = true;
                    Destroy(gameObject, 1f);
                }
            }
        }
    }
}