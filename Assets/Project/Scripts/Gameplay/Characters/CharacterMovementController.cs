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
        public CharacterAction MovementActionTest;
        public CharacterAction StopMovementAction;
        public CharacterAction SpeedUpAction;
        public CharacterStatesProfile CharacterStatesProfile;
        public GameCombatController GameCombatController;
        public CharacterCombatController CharacterCombatController { private set; get; }
        private CharacterStateController _characterStateController;
        public LayerMask Layer;
        private NavMeshAgent _navMeshAgent;
        public float DefaultMeleeRangeTest = 5f;
        void Start()
        {
            Initialize(null);
        }

        void Update()
        {
            ProcessStoppingAndInformStateController();
            
            if(Input.GetKeyUp(KeyCode.F) == true)
            {
                _characterStateController.TriggerCharacterAction(SpeedUpAction);
            }
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
            if(_navMeshAgent == null)
            {
                returnValue = false;
            }

            SubscribeToCharacterActionTriggeredEvent();
            return returnValue;
        }


        public bool MoveToWorldPoint(Vector3 targetPoint)
        {
            if(_characterStateController.IsActionAllowed(MovementActionTest) == false)
            {
                return false;
            }
            _navMeshAgent.isStopped = false;
            var navMeshMoveResult = _navMeshAgent.SetDestination(targetPoint);
            if (navMeshMoveResult == true)
            {
                RequestStateChange(_characterStateController.CharacterStatesProfile.Walking);
            }

            return navMeshMoveResult;
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