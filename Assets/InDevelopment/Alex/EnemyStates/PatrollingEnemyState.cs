using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InDevelopment.Alex.EnemyStates
{
    public class PatrollingEnemyState : EnemyStateBase
    {
        /// <summary>
        /// What does this state need?
        /// Target(s), maybe through a simple waypoint system?
        /// </summary>
        
        [Tooltip("This is how close this object has to be to it's 'target' before it moves on to the next bit of code.")]
        public float minDistancedForIsReached = 0.5f;
        public PatrolType patrolType = PatrolType.Ordered;
        public List<GameObject> waypoints;
        
        
        private int targetIndex = 0;
        private int targetIndexMax;
        private EnemyController _enemyController;

        private GameObject _target;

        private Vector3 SpawnPosition;
        public enum PatrolType
        {
            Ordered,
            Random
        }
        private void Awake()
        {
            _target = waypoints[targetIndex];
            _enemyController = GetComponentInParent<EnemyController>();
            SpawnPosition = _enemyController.transform.position;
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.GetComponent<EnemyController>();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();

            if (!IsReached())
            {
                _enemyController.MoveToTarget(GetTargetPosition());
            }
            else
            {

                if (stateManager.currentEnemyState != _enemyController.waitingAtPointEnemyState)
                {
                    stateManager.ChangeState(_enemyController.waitingAtPointEnemyState);
                }
                GetTarget();
            }
        }

        private GameObject GetTarget()
        {
            if (waypoints == null)
            {
                Debug.Log("PatrollingState: Waypoints is null.");
                return null;
            }
            targetIndexMax = waypoints.Count - 1;

            if (patrolType == PatrolType.Ordered)
            {
                if (targetIndex < targetIndexMax)
                {
                    targetIndex++;
                
                }
                else if (targetIndex >= targetIndexMax)
                {
                    targetIndex = 0;
                }
            }

            if (patrolType == PatrolType.Random)
            {
                targetIndex = Random.Range(0, (targetIndexMax + 1));
            }
            
            _target = waypoints[targetIndex];
            GetTargetPosition();
            return waypoints[targetIndex];
        }
        
        private Vector3 GetTargetPosition()
        {
            if (waypoints != null)
            {
                return waypoints[targetIndex].transform.position;
            }

            return Vector3.zero;
        }
        
        public bool IsReached()
        {
            float dist = (Vector3.Distance(_enemyController.transform.position, _target.transform.position));
            if (dist <= minDistancedForIsReached)
            {
                return true;
            }
            return false;
        }

        
        
    }
}
