using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Camera
{
    public class CameraMovementController : MonoBehaviour
    {
        [SerializeField]
        protected CharacterMovementController _characterMovementController;
        protected Transform _playerTransform;
        private Vector3 _offset;
        // Start is called before the first frame update
        void Start()
        {
            _playerTransform = _characterMovementController.gameObject.transform;
            _offset = gameObject.transform.position - _playerTransform.position;
        }

        // Update is called once per frame
        void Update()
        {
            FollowCharacterMovement();
        }

        private void FollowCharacterMovement()
        {
            gameObject.transform.position = _offset + _playerTransform.position;
        }
    }
}