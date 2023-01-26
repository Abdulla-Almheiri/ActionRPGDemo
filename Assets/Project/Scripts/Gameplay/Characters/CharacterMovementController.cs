using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chaos.Systems;
using Chaos.Gameplay.Systems;
using UnityEngine.EventSystems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterMovementController : MonoBehaviour
    {
        [field:SerializeField]
        public CharacterMovementTemplate CharacterMovementTemplate { private set; get; }
        public float BaseSpeed { private set; get; } = 0f;
        private CharacterAnimationController _characterAnimationController;
        private CharacterCombatController _characterCombatController;
        private NavMeshAgent _navMeshAgent;
        private LayerMask _layer;


        void Awake()
        {
            Initialize(null);
        }

        void Update()
        {
            ProcessNavMeshRotation();

            _characterAnimationController.Animator.SetFloat("Velocity", _navMeshAgent.velocity.magnitude/_navMeshAgent.speed);
        }

        private void ProcessNavMeshRotation()
        {
            if(_navMeshAgent.enabled == false)
            {
                return;
            }

            if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance / 2f)
            {
                _navMeshAgent.updateRotation = false;
            }
        }

        public void Initialize(Game game)
        {
            _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            _characterCombatController = GetComponent<CharacterCombatController>();
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            _characterCombatController = GetComponent<CharacterCombatController>();
            _navMeshAgent.speed = CharacterMovementTemplate.BaseMovementSpeed;
            BaseSpeed = _navMeshAgent.speed;
            _layer = CharacterMovementTemplate.GroundLayer;

        }

        public void DisableNavMeshComponent()
        {
            if(_navMeshAgent == null)
            {
                return;
            }

            _navMeshAgent.enabled = false;
        }

        public void EnableNavMeshComponent()
        {
            if (_navMeshAgent == null)
            {
                return;
            }

            _navMeshAgent.enabled = true;
        }
        public bool MoveToWorldPoint(Vector3 targetPoint)
        {
            if (_characterCombatController.Alive == false)
            {
                return false ;
            }

            if ((targetPoint - transform.position).sqrMagnitude < _navMeshAgent.stoppingDistance)
            {
                return false;
            }

            if (_navMeshAgent.SetDestination(targetPoint))
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.updateRotation = true;
            }
            
            return true;
        }
        public bool IsRunning()
        {
            return _navMeshAgent.velocity.magnitude > 0.01f;
        }

        public void MoveToMousePosition()
        {
            if(_characterCombatController.Alive == false)
            {
                return;
            }

            if(EventSystem.current.IsPointerOverGameObject() == true)
            {
                Debug.Log("Pointer over gameobject : " + EventSystem.current.name );
                return;
            }

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, _layer) == true)
            {
                MoveToWorldPoint(rayHit.point);
            }

        }

        public void StopMovement()
        {
            if(_navMeshAgent == null || _navMeshAgent.enabled == false)
            {
                return;
            }

            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;
        }
        public void MoveToGameObject(GameObject targetGameObject)
        {
            MoveToWorldPoint(targetGameObject.gameObject.transform.position);
        }

        public float GetCurrentMovementSpeed()
        {
            var currentSpeed = _navMeshAgent.velocity.magnitude / _navMeshAgent.speed;
            return currentSpeed;
        }

        public void RotateCharacterXZTowardsPoint(Vector3 point)
        {
            if(_navMeshAgent.enabled == false)
            {
                return;
            }

            //Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
             Ray ray = UnityEngine.Camera.main.ViewportPointToRay(point);
             RaycastHit rayHit;

             if (Physics.Raycast(ray, out rayHit, _layer))
             {
                 var direction = (rayHit.point - transform.position);
                 direction.y = 0;
                 direction = direction.normalized;
                 transform.rotation = Quaternion.LookRotation(direction);
             }

        }

        public void FaceDirectionOfCharacter(CharacterMovementController character)
        {
            if(_navMeshAgent == null || _navMeshAgent.enabled == false)
            {
                return;
            }

            var direction = character.transform.position;
            direction.y = 0;
            transform.LookAt(direction);
        }

        public void RotateCharacterInMouseDirection()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, _layer))
            {
                var direction = (rayHit.point - transform.position);
                direction.y = 0;
                direction = direction.normalized;
                transform.rotation = Quaternion.LookRotation(direction);

            }
        }

        public float GetUnsignedDistanceBetweenCharacters(CharacterMovementController targetCharacter)
        {
            if(targetCharacter == null)
            {
                return 0;
            }
            return Mathf.Abs((gameObject.transform.position - targetCharacter.gameObject.transform.position).magnitude);
        }

        public bool IsCharacterWithinMeleeRange(CharacterMovementController targetCharacter)
        {
            float distance = GetUnsignedDistanceBetweenCharacters(targetCharacter);
            float combinedMeleeRange = _characterCombatController.CharacterCombatTemplate.Size + targetCharacter._characterCombatController.CharacterCombatTemplate.Size;
            if (distance <= combinedMeleeRange)
            {
                return true;
            } else
            {
                return false;
            }
        }

    }
}