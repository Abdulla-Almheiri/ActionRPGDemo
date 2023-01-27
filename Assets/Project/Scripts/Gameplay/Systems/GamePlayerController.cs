using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using UnityEngine.SceneManagement;

namespace Chaos.Gameplay.Systems
{
    public class GamePlayerController : MonoBehaviour
    {
        public CharacterMovementController PlayerMovementController;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}