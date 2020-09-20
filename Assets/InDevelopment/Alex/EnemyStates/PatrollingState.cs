using System;
using System.Collections.Generic;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class PatrollingState : StateBase
    {
        /// <summary>
        /// What does this state need?
        /// Target(s), maybe through a simple waypoint system?
        /// </summary>

        public float minDistancedForIsReached = 0.5f;

        public List<GameObject> waypoints;
        private int targetIndex = 0;
        private int targetIndexMax;
        private EnemyController _enemyController;

        private GameObject _target;

        private Vector3 SpawnPosition;

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
                GetTarget();
            }
        }

        private GameObject GetTarget()
        {
            if (waypoints == null)
                return null;
            
            targetIndexMax = waypoints.Count - 1;

            if (targetIndex < targetIndexMax)
            {
                targetIndex++;
                
            }
            else if (targetIndex >= targetIndexMax)
            {
                targetIndex = 0;
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
        
        bool IsReached()
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
