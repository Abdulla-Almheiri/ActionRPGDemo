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
        void Start()
        {
            Initialize(null);
        }

        void Update()
        {
            ProcessStoppingAndInformStateController();
            
            if(Input.GetKeyUp(KeyCode.F) == true)
            {
                _characterAnimationController.Animator.speed = 0f;
            }

            if (Input.GetKeyUp(KeyCode.G) == true)
            {
                _characterAnimationController.Animator.speed = 1f;
            }
            /*if(IsRunning())
            {
                _characterAnimationController.Animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
            }*/
            //Debug.Log("Animator playback speed     is   :     " + _characterAnimationController.Animator.speed);

            //RotateCharacterXZTowardsPoint(_navMeshAgent.transform.position);


            if (Mathf.Abs((_navMeshAgent.nextPosition - transform.position).magnitude) <= _navMeshAgent.radius*2)
            {

            }

            _characterAnimationController.Animator.SetFloat("MovementSpeed", _navMeshAgent.velocity.normalized.magnitude);
        }

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
            /* if(_characterStateController.IsActionAllowed(MovementActionTest) == false)
             {
                 return false;
             }
             _navMeshAgent.isStopped = false;
             var navMeshMoveResult = _navMeshAgent.SetDestination(targetPoint);
             if (navMeshMoveResult == true)
             {
                 RequestStateChange(_characterStateController.CharacterStatesProfile.Walking);
             }

             return navMeshMoveResult;*/

            /* if(_navMeshAgent.SetDestination(targetPoint))
             {
                 _destination = targetPoint;
                 _onPath = true;
                 _characterAnimationController.PlayCharacterAnimationFromCharacterState(CharacterStatesProfile.Walking, 0.2f);
             } 
            */

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(targetPoint);
            //RotateCharacterInMouseDirection();
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
                StopMovment();
            }
            Debug.Log("Action triggered by event     :   " + characterAction.name);
        }

        private void StopMovment()
        {
            if(_navMeshAgent == null)
            {
                return;
            }

            _navMeshAgent.isStopped = true;
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