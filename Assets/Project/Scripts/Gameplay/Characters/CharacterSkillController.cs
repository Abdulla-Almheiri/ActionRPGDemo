using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterSkillController : MonoBehaviour
    {
        public GameUIController GameUIController;
        public GameObject SkillVFX;
        public CharacterVFXSpawnLocationType SpawnLocationType;

        protected CharacterVFXController _characterVFXController;
        protected CharacterMovementController _characterMovementController;

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
            _characterMovementController = GetComponent<CharacterMovementController>();
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
            spawnedVFX.GetComponent<SkillPrefabCombatController>().GameUIControllerTest = GameUIController;
            
        }

        
    }
}