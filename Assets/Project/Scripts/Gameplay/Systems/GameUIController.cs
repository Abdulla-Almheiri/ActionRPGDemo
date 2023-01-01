using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using TMPro;
using Chaos.Gameplay.UI;

namespace Chaos.Gameplay.Systems
{
    public class GameUIController : MonoBehaviour
    {
        public GameObjectPoolTemplate GameObjectPoolTemplate;
        private GamePoolController _gamePoolController;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initialize()
        {
            _gamePoolController = GetComponent<GamePoolController>();
            
        }

        public void SpawnDamageTextAtScreenPositionTest(float amount, Vector3 targetScreenPosition)
        {
            var objPool = _gamePoolController.RetrieveGameObjectPoolByTemplate(GameObjectPoolTemplate);
            var spawn = objPool.RetrieveFirstAvailablePooledGameObject();
            if(spawn == null)
            {
                return;
            }
            spawn.gameObject.SetActive(true);
            spawn.gameObject.GetComponent<FloatingCombatTextAnimationController>().Initialize(targetScreenPosition);
            //spawn.gameObject.GetComponent<TMP_Text>().rectTransform.SetPositionAndRotation(targetScreenPosition, Quaternion.identity);
            spawn.gameObject.GetComponent<TMP_Text>().text = amount.ToString();


        }
    }
}
