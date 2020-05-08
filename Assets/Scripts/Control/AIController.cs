using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float dwellingTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float WaypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Health health;

        GameObject player;
        Mover mover;

        int currentWawpointIndex = 0;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastWaypoint = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            guardPosition = transform.position;

            
        }

        private void Update()
        {
            if (health.IsDead()) return;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {

               
                AttackBehavior(player);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }

            else
            {
                PatrolBehavior();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceLastWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceLastWaypoint > dwellingTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
           
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWawpointIndex);
        }

        private void CycleWaypoint()
        {
            currentWawpointIndex = patrolPath.GetNextIndex(currentWawpointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < WaypointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().cancelCurrentAction();
        }

        private void AttackBehavior(GameObject player)
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
           float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
            // Mover().MoveTo at chase speed
        }
      
        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}
