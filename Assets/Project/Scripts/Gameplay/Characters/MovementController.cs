using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chaos.Systems;

namespace Chaos.Gameplay.Characters
{
    public class MovementController : GameController
    {
        public LayerMask Layer;
        private NavMeshAgent _navMeshAgent;

        void Start()
        {
            Initialize(null);
        }

        void Update()
        {
            if(Input.GetMouseButton(0) == true)
            {
                MoveToMousePosition();
                
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

            if(_navMeshAgent == null)
            {
                returnValue = false;
            }

            return returnValue;
        }


        public bool MoveToWorldPoint(Vector3 targetPoint)
        {
            _navMeshAgent.isStopped = false;
            var navMeshMoveResult = _navMeshAgent.SetDestination(targetPoint);
            if (navMeshMoveResult == true)
            {

            }
            return navMeshMoveResult;
        }

        public bool IsRunning()
        {
            return false;
        }

        public void MoveToMousePosition()
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Layer) == true)
            {
                MoveToWorldPoint(rayHit.point);
            }

        }

        public void StopMovement()
        {
            _navMeshAgent.isStopped = true;
        }

        public void MoveToCharacter(CharacterController character)
        {

        }

        public float GetCurrentMovementSpeed()
        {
            var currentSpeed = _navMeshAgent.velocity.magnitude / _navMeshAgent.speed;
            return currentSpeed;
        }

        public void RotateCharacterInMouseDirection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, Layer))
            {
                var direction = (rayHit.point - transform.position);
                direction.y = 0;
                direction = direction.normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}