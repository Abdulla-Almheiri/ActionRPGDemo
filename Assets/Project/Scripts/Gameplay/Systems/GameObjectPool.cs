using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

namespace Chaos.Gameplay.Systems
{
    public class GameObjectPool
    {
        private Stack<GameObjectPoolController> _readyObjects = new Stack<GameObjectPoolController>();
        private Queue<GameObjectPoolController> _objectsInUse = new Queue<GameObjectPoolController>();
        private GameObjectPoolTemplate _gameObjectPoolTemplate;
        private GamePoolController _gamePoolController;
        private int _maxSize = 0;
        public GameObjectPool(GameObjectPoolTemplate gameObjectPoolTemplate, GamePoolController gamePoolController)
        {
            Initialize(gameObjectPoolTemplate, gamePoolController);
        }

        private void Initialize(GameObjectPoolTemplate gameObjectPoolTemplate, GamePoolController gamePoolController)
        {
            //Prevents multiple initializations
            if (_gameObjectPoolTemplate != null)
            {
                return;
            }

            _gameObjectPoolTemplate = gameObjectPoolTemplate;

            //No template provided
            if(_gameObjectPoolTemplate == null)
            {
                return;
            }
            _maxSize = _gameObjectPoolTemplate.MaxSize;
            
            if(_gameObjectPoolTemplate.PoolablePrefab == null)
            {
                return;
            }

            _gamePoolController = gamePoolController;

            if(_gamePoolController == null)
            {
                return;
            }

            if (_gameObjectPoolTemplate.PreAllocateObjects == true)
            {
                AllocateSpaceForPooledObjects(_maxSize);
            }

        }

       
        public void ReturnObjectToPool(GameObjectPoolController pooledGameObject)
        {
            if(_readyObjects.Count >= _maxSize)
            {
                return;
            }

            pooledGameObject.gameObject.SetActive(false);
            _objectsInUse.Dequeue();
            _readyObjects.Push(pooledGameObject);
        }

        public GameObjectPoolController RetrieveFirstAvailablePooledGameObject(bool setActive = true)
        {
            if(_readyObjects.Count <= 0 && _objectsInUse.Count == _maxSize)
            {
                return null;
            }

           /* if(_objectsInUse.Count == _maxSize)
            {
                
                return null;
            }*/

            if(_readyObjects.Count == 0)
            {
                AllocateSpaceForPooledObjects(1);
            }

            var retrievedObject = _readyObjects.Pop();
            _objectsInUse.Enqueue(retrievedObject);
            if(_gameObjectPoolTemplate.HideActiveInHierarchy == true)
            {
                retrievedObject.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }

            if(setActive == true)
            {
                retrievedObject.gameObject.SetActive(true);
            }

            return retrievedObject;
        }

        private int AvailableSlotsInPool()
        {
            if(_maxSize == 0)
            {
                
                return 0;
            }

            var slots = _maxSize - (_readyObjects.Count + _objectsInUse.Count);
            
            return slots;
        }

        private void AllocateSpaceForPooledObjects(int requestedNumberOfSlotsToAllocate)
        {
            
            int slotsToAllocate = Mathf.Min(AvailableSlotsInPool() , requestedNumberOfSlotsToAllocate);

            if (slotsToAllocate <= 0)
            {
                return;
            }

            for (int i = 0; i < slotsToAllocate; i++)
            {
                
                GameObjectPoolController spawn;
                if (_gameObjectPoolTemplate.RequiresCanvas == true)
                {
                    spawn = GameObject.Instantiate(_gameObjectPoolTemplate.PoolablePrefab, _gamePoolController.DynamicCanvasTest.transform);
                    
                }
                else
                {
                    spawn = GameObject.Instantiate(_gameObjectPoolTemplate.PoolablePrefab);
                }
                
                spawn.Initialize(this);
                if (_gameObjectPoolTemplate.HideInactiveInHierarchy == true)
                {
                    spawn.gameObject.hideFlags = HideFlags.HideInHierarchy;
                }
                if (_gameObjectPoolTemplate.SetActiveAfterAllocation == false)
                {
                    spawn.gameObject.SetActive(false);
                } else
                {
                    spawn.gameObject.SetActive(true);
                }

                _readyObjects.Push(spawn);

            }

        }
    }
}