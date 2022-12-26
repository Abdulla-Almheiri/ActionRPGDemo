using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Skills;

namespace Chaos.Gameplay.Characters
{
    public class SkillController : MonoBehaviour
    {
        public GameObject SkillVFX;
        public CharacterVFXSpawnLocationType SpawnLocationType;

        protected CharacterVFXController _characterVFXController;
        protected MovementController _characterMovementController;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void Initialize()
        {
            _characterVFXController = GetComponent<CharacterVFXController>();
            _characterMovementController = GetComponent<MovementController>();
        }
        public void SpawnSkillVFXTest()
        {
            if(SkillVFX == null || _characterVFXController == null || SpawnLocationType == null)
            {
                return;
            }
            _characterMovementController.RotateCharacterInMouseDirection();
            _characterMovementController.StopMovement();
            var spawnPoint = _characterVFXController.GetVFXSpawnTransform(SpawnLocationType);

            if(spawnPoint == null)
            {
                return;
            }
            var spawnedVFX = Instantiate(SkillVFX, spawnPoint);
            spawnedVFX.transform.SetParent(null);
            
        }

        
    }
}