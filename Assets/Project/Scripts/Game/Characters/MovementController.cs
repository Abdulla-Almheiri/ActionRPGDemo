using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game;
using Game.Systems;

namespace Game.Characters
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

            if (Physics.Raycast(ray, out rayHit, Layer))
            {
                MoveToWorldPoint(rayHit.point);
            }

        }

        public void StopMovement()
        {

        }

        public void MoveToCharacter(CharacterController character)
        {

        }

    }
}