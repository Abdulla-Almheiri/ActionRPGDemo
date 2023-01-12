using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Camera
{
    public class CameraMovementController : MonoBehaviour
    {
        public float LerpSpeedTest = 0.1f;
        [SerializeField]
        private GamePlayerController _gamePlayerController;
        protected CharacterMovementController _characterMovementController;
        protected Transform _playerTransform;
        private Vector3 _offset;

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _characterMovementController = _gamePlayerController.PlayerMovementController;
            _playerTransform = _characterMovementController.gameObject.transform;
            _offset = gameObject.transform.position - _playerTransform.position;
        }

        void Update()
        {
            FollowCharacterMovement();
        }

        private void FollowCharacterMovement()
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, _offset + _playerTransform.position, LerpSpeedTest);
        }
    }
}