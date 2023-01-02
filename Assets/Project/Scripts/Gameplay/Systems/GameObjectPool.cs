using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

namespace Chaos.Gameplay.Systems
{
    public class GameObjectPool
    {
        private Stack<GameObjectPoolController> _readyObjects = new Stack<GameObjectPoolController>();

        private List<GameObjectPoolController> _objectsInUse = new List<GameObjectPoolController>();

        private GameObjectPoolTemplate _gameObjectPoolTemplate;
        private GamePoolController _gamePoolController;
        private int _maxSize = 0;
        private int _allocatedObjectsCount = 0;
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
                //AllocateSpaceForPooledObjects(_maxSize);
                PreAllocateAndInstantiateGameObjects(_maxSize);
            }

        }

       
        public void ReturnObjectToPool(GameObjectPoolController pooledGameObject)
        {
            if(_gameObjectPoolTemplate == null)
            {
                return;
            }

            if(_objectsInUse.Remove(pooledGameObject) == false)
            {
                return;
            }


            _readyObjects.Push(pooledGameObject);
        }

        

        public GameObjectPoolController RetrieveNextAvailableGameObjectFromPool()
        {
            if(_allocatedObjectsCount >= _maxSize && _readyObjects.Count == 0)
            {
                
                return null;
            }

            if (_readyObjects.Count > 0)
            {
                
                var obj = _readyObjects.Pop();
                ApplyTemplateFlagsToGameObject(obj);
                _objectsInUse.Add(obj);
                //obj.gameObject.SetActive(true);
                return obj;
            }

            if(PreAllocateAndInstantiateGameObjects(1) == true)
            {
                
                var obj = _readyObjects.Pop();
                ApplyTemplateFlagsToGameObject(obj);
                _objectsInUse.Add(obj);
                return obj;
            }

            return null;

        }

        

        private int NumberOfFreeGameObjects()
        {
            if(_maxSize == 0)
            {
                
                return 0;
            }

            return _readyObjects.Count;
        }

        private bool PreAllocateAndInstantiateGameObjects(int numberOfObjectsToPreAllocate)
        {
            if(_allocatedObjectsCount >= _maxSize)
            {
                return false;
            }

            int iterations = Mathf.Min(RemainingUnallocatedSlots(), numberOfObjectsToPreAllocate);
            
            for(int i =0; i<iterations; i++)
            {
                var spawn = InstantiateNewGameObjectByTemplate();
                InitializeGameObjectToThisGamePool(spawn);
                ApplyTemplateFlagsToGameObject(spawn);
                _readyObjects.Push(spawn);
                _allocatedObjectsCount++;
            }

            return iterations > 0;
        }


        private GameObjectPoolController InstantiateNewGameObjectByTemplate()
        {
            if(_gameObjectPoolTemplate == null)
            {
                return null;
            }

            GameObjectPoolController spawn;
            if (_gameObjectPoolTemplate.RequiresCanvas == true)
            {
                spawn = GameObject.Instantiate(_gameObjectPoolTemplate.PoolablePrefab, _gamePoolController.DynamicCanvasTest.transform);

            }
            else
            {
                spawn = GameObject.Instantiate(_gameObjectPoolTemplate.PoolablePrefab);
            }

            return spawn;
        }
        private void InitializeGameObjectToThisGamePool(GameObjectPoolController gameObjectToInitialize)
        {
            gameObjectToInitialize.Initialize(this);
        }

        private void ApplyTemplateFlagsToGameObject(GameObjectPoolController gameObjectToApplyTemplateFlagsTo)
        {
            if(_gameObjectPoolTemplate == null)
            {
                return;
            }

            if(gameObjectToApplyTemplateFlagsTo == null)
            {
                return;
            }


            if (_gameObjectPoolTemplate.HideInactiveInHierarchy == true)
            {
                gameObjectToApplyTemplateFlagsTo.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }

            if (_gameObjectPoolTemplate.SetActiveAfterAllocation == false)
            {
                gameObjectToApplyTemplateFlagsTo.gameObject.SetActive(false);
            }
            else
            {
                gameObjectToApplyTemplateFlagsTo.gameObject.SetActive(true);
            }
        }
        private int RemainingUnallocatedSlots()
        {
            return _maxSize - (_objectsInUse.Count + _readyObjects.Count);
        }

        private int NumberOfGameObjectsAvailableForUse()
        {
            return _readyObjects.Count;
        }

    }
}