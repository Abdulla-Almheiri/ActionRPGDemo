using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Systems
{
    public class GameObjectPoolController : MonoBehaviour
    {
        private GameObjectPool _gameObjectPool;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initialize(GameObjectPool gameObjectPool)
        {
            //Prevents multiple initializations
            if (_gameObjectPool != null)
            {
                return;
            }

            _gameObjectPool = gameObjectPool;

            //No GameObjectPool provided
            if(_gameObjectPool == null)
            {
                return;
            }


        }

        public void ReturnToPool()
        {
            if(_gameObjectPool == null)
            {
                return;
            }

            _gameObjectPool.ReturnObjectToPool(this);
        }

        private void OnDisable()
        {
            ReturnToPool();
        }
    }
}