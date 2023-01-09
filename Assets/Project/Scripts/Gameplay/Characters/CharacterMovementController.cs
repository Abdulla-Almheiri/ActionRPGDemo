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
        void Start()
        {
            Initialize(null);
        }

        void Update()
        {
            //ProcessStoppingAndInformStateController();
            SyncAgentAndAnimator();
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



            if(_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            {
                _characterAnimationController.Animator.SetBool("IsMoving", false);
            } else
            {
                transform.rotation = Quaternion.LookRotation(_navMeshAgent.nextPosition);
            }

            var distance = Vector3.Distance(transform.position, _navMeshAgent.nextPosition);
            Debug.Log("Distance is         :    " + distance);

            _characterAnimationController.Animator.SetFloat("MovementSpeed", distance);
        }

        public void OnAnimatorMove()
        {
            
            Vector3 rootPosition = _characterAnimationController.Animator.rootPosition;
            rootPosition.y = _navMeshAgent.nextPosition.y;
            //_navMeshAgent.transform.position = rootPosition;
            //transform.LookAt(_navMeshAgent.nextPosition);
            transform.position = rootPosition;
            /*if ((transform.position - _navMeshAgent.nextPosition).magnitude > 3f )
            {
                transform.LookAt(_navMeshAgent.nextPosition);
            } else
            {
                _characterAnimationController.Animator.CrossFadeInFixedTime("StopRun", 0.15f);
            }*/

            /*if((_navMeshAgent.destination - transform.position).magnitude < _navMeshAgent.stoppingDistance/2f)
            {
                Debug.Log("Magnitutde is      :   " + (_navMeshAgent.transform.position - transform.position).magnitude);
                _characterAnimationController.Animator.CrossFadeInFixedTime("Idle", 0.15f);
            }*/
            /*transform.position = rootPosition;
            _navMeshAgent.nextPosition = rootPosition;
            transform.rotation = _characterAnimationController.Animator.rootRotation;*/

            /*bool shouldMove = _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;
            Vector2 velocity = _characterAnimationController.Animator.deltaPosition * _navMeshAgent.speed * _navMeshAgent.speed;
            _characterAnimationController.Animator.SetTrigger("Move");
            _characterAnimationController.Animator.SetFloat("Movement", velocity.magnitude);*/
        }
        private void SyncAgentAndAnimator()
        {
            /*Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;
            worldDeltaPosition.y = 0f;
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

            _velocity = SmoothDeltaPosition / Time.deltaTime;
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(
                    Vector2.zero,
                    _velocity,
                    _navMeshAgent.remainingDistance / _navMeshAgent.stoppingDistance
                    );
            }

            bool shouldMove = _velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;


            _characterAnimationController.Animator.SetBool("IsMoving", shouldMove);
            _characterAnimationController.Animator.SetFloat("Movement", _velocity.magnitude);

            float deltaMagnitude = worldDeltaPosition.magnitude;
            if(deltaMagnitude > _navMeshAgent.radius /2f)
            {
                transform.position = Vector3.Lerp(_characterAnimationController.Animator.rootPosition,
                    _navMeshAgent.nextPosition,
                    smooth);
            }*/



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
            _navMeshAgent.updatePosition = false;
            //_navMeshAgent.updateRotation = false;
            SubscribeToCharacterActionTriggeredEvent();
            return returnValue;
        }

        public bool MoveToWorldPoint(Vector3 targetPoint)
        {
            if((targetPoint - transform.position).magnitude < _navMeshAgent.stoppingDistance)
            {
                Debug.Log("Point Ignored");
                return false;
            }
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

            //_navMeshAgent.isStopped = false;
            if(_navMeshAgent.SetDestination(targetPoint))
            {
                _characterAnimationController.Animator.SetBool("IsMoving", true);
            }
            //transform.LookAt(targetPoint);
           // _characterAnimationController.Animator.CrossFadeInFixedTime("Walk", 0.2f);
            
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