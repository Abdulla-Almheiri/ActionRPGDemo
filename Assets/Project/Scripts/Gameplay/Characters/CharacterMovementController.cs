using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chaos.Systems;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Characters
{
    public class CharacterMovementController : GameController
    {
        [SerializeField]
        private Transform _rootCharacterTransform;
        private Vector3 _initialRootCharacterPosition;
        private Quaternion _initialRootCharacterRotation;
        public CharacterAction MovementActionTest;
        public CharacterAction StopMovementAction;
        public CharacterAction SpeedUpAction;
        public CharacterStatesProfile CharacterStatesProfile;
        public GameCombatController GameCombatController;
        public CharacterCombatController CharacterCombatController { private set; get; }
        private CharacterStateController _characterStateController;
        private CharacterAnimationController _characterAnimationController;
        public LayerMask Layer;
        private NavMeshAgent _navMeshAgent;
        public float DefaultMeleeRangeTest = 5f;
        private Vector3 _destination;
        private bool _onPath = false;
        private float _baseSpeed = 0f;

        private Vector2 _velocity;
        private Vector2 SmoothDeltaPosition;

        private Vector3 _nextPoint;
        private bool _isMoving = false;
        private int _pathCornerIndex = 0;

        void Start()
        {
            Initialize(null);
        }

        void Update()
        {
            if(_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance/2f)
            {
                _navMeshAgent.updateRotation = false;
                //_characterAnimationController.Animator.CrossFadeInFixedTime("Idle", 0.2f);
            }
            _characterAnimationController.Animator.SetFloat("Velocity", _navMeshAgent.velocity.magnitude/_navMeshAgent.speed);
        }

      /*  public void OnAnimatorMove()
        {
            if(_navMeshAgent.SetDestination(_characterAnimationController.Animator.deltaPosition))
            {
                transform.position = _characterAnimationController.Animator.rootPosition;
                _rootCharacterTransform.position -= _characterAnimationController.Animator.deltaPosition;
            } else
            {
                
            }
        }*/

        public override bool Initialize(Game game)
        {
            var returnValue = base.Initialize(game);
            /*if(returnValue == false)
            {
                return false;
            }*/

            _navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            CharacterCombatController = GetComponent<CharacterCombatController>();
            _characterStateController = GetComponent<CharacterStateController>();
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            if(_navMeshAgent == null)
            {
                returnValue = false;
            }

            _baseSpeed = _navMeshAgent.speed;
            SubscribeToCharacterActionTriggeredEvent();
            return returnValue;
        }

        public bool MoveToWorldPoint(Vector3 targetPoint)
        {
            if((targetPoint - transform.position).sqrMagnitude < _navMeshAgent.stoppingDistance)
            {
                return false;
            }

            if (_navMeshAgent.SetDestination(targetPoint))
            {
                _navMeshAgent.updateRotation = true;
                _navMeshAgent.isStopped = false;
            }
            
            return true;
        }

        private void RequestStateChange(CharacterState newCharacterState )
        {
            if(_characterStateController == null)
            {
                return;
            }

            _characterStateController.AttemptCharacterStateChange(newCharacterState, 0.5f);

        }
        public bool IsRunning()
        {
            return _navMeshAgent.velocity.magnitude > 0.01f;
        }

        public void ProcessStoppingAndInformStateController()
        {
            if(_characterStateController == null)
            {
                return;
            }

            if (IsRunning() == false)
            {
                RequestStateChange(CharacterStatesProfile.Idle);
            }
        }
        public void MoveToMousePosition()
        {

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Layer) == true)
            {
                MoveToWorldPoint(rayHit.point);
            }

        }

        /*private void RequestStateChangeToIdle()
        {
            if(_characterStateController == null)
            {
                return;
            }

            _characterStateController.SetCurrentCharacterStateWithAnyTransitionType(_characterStateController.CharacterStatesProfile.Idle);
        }*/
        /*public void StopMovementAndRequestStateChangeToIdle()
        {
            
            _navMeshAgent.isStopped = true;
        }*/
        private void SubscribeToCharacterActionTriggeredEvent()
        {
            if(_characterStateController == null)
            {
                return;
            }

            _characterStateController.SubscribeToCharacterActionTriggered(OnCharacterActionTriggered);
        }

        private void UnsubscribeToCharacterActionTriggeredEvent()
        {
            if (_characterStateController == null)
            {
                return;
            }

            _characterStateController.UnsubscribeToCharacterActionTriggered(OnCharacterActionTriggered);
        }

        private void OnCharacterActionTriggered(CharacterAction characterAction)
        {
            if(characterAction == StopMovementAction)
            {
               // StopMovment();
            }
            //Debug.Log("Action triggered by event     :   " + characterAction.name);
        }

        public void StopMovement()
        {
            if(_navMeshAgent == null)
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
            //Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
             Ray ray = UnityEngine.Camera.main.ViewportPointToRay(point);
             RaycastHit rayHit;

             if (Physics.Raycast(ray, out rayHit, Layer))
             {
                 var direction = (rayHit.point - transform.position);
                 direction.y = 0;
                 direction = direction.normalized;
                 transform.rotation = Quaternion.LookRotation(direction);
             }

        }

        public void RotateCharacterInMouseDirection()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Layer))
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
            //Debug.Log("Distance   is    :    " + distance);
            float combinedMeleeRange = CharacterCombatController.CharacterCombatTemplate.Size + targetCharacter.CharacterCombatController.CharacterCombatTemplate.Size;
            if (distance <= combinedMeleeRange)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void OnDisable()
        {
            UnsubscribeToCharacterActionTriggeredEvent();
        }
    }
}