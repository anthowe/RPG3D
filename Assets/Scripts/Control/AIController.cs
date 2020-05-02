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
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float WaypointTolerance = 1f;

        Fighter fighter;
        Health health;

        GameObject player;
        Mover mover;


        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

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
                
                timeSinceLastSawPlayer = 0;
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
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            mover.StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWaypoint()
        {
            throw new NotImplementedException();
        }

        private void CycleWaypoint()
        {
            throw new NotImplementedException();
        }

        private bool AtWaypoint()
        {
            throw new NotImplementedException();
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().cancelCurrentAction();
        }

        private void AttackBehavior(GameObject player)
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
           float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
      
        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}
