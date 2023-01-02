using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Chaos.Gameplay.Systems
{
    public class GamePoolController : MonoBehaviour
    {
        public List<GameObjectPoolTemplate> _gameObjectPoolTemplatesList = new List<GameObjectPoolTemplate>();
        public GameObjectPoolTemplate PoolTemplateTest;
        public Canvas DynamicCanvasTest;

        private Dictionary<GameObjectPoolTemplate, GameObjectPool> _gameObjectPoolTemplatesDictionary = new Dictionary<GameObjectPoolTemplate, GameObjectPool>();
        public GameObjectPool GameObjectPool { private set; get; }

        // Start is called before the first frame update
        void Awake()
        {
            Initialize(_gameObjectPoolTemplatesList);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.P))
            {
                var objPool = RetrieveGameObjectPoolByTemplate(PoolTemplateTest);
                var spawn = objPool.RetrieveNextAvailableGameObjectFromPool();
                spawn?.gameObject.SetActive(true);

            }
        }

        public void Initialize(List<GameObjectPoolTemplate> gameObjectPoolTemplatesList)
        {
            foreach(GameObjectPoolTemplate template in gameObjectPoolTemplatesList)
            {
                if(template == null)
                {
                    continue;
                }
                GameObjectPool gameObjectPool = new GameObjectPool(template, this);
                _gameObjectPoolTemplatesDictionary[template] = gameObjectPool;
            }

        }

        public void SpawnPooledObjectFromControllerTest()
        {
            var spawn = Instantiate(PoolTemplateTest.PoolablePrefab, DynamicCanvasTest.transform);
            spawn.GetComponent<TMP_Text>().text = "HELLO";
        }

        public GameObjectPool RetrieveGameObjectPoolByTemplate(GameObjectPoolTemplate gameObjectTemplate)
        {
            GameObjectPool retrievedGameObjectPool;
            if(_gameObjectPoolTemplatesDictionary.TryGetValue(gameObjectTemplate, out retrievedGameObjectPool) == true)
            {
                return retrievedGameObjectPool;
            } else
            {
                return null;
            }
        }
    }
}