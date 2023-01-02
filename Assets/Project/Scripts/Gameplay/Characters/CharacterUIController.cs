using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterUIController : MonoBehaviour
    {
        public GameUIController GameUIControllerTest;

        private CharacterCombatController _characterCombatController;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }


        private void Initialize()
        {
            _characterCombatController.GetComponent<CharacterCombatController>();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}