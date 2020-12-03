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
        [Tooltip(
            "This is how close this object has to be to it's 'target' before it moves on to the next bit of code.")]
        public float minDistancedForIsReached = 0.5f;

        public PatrolType patrolType = PatrolType.Ordered;
        public List<GameObject> waypoints;


        private int targetIndex = 0;
        private int targetIndexMax;
        private GameObject target;

        private Vector3 SpawnPosition;

        public enum PatrolType
        {
            Ordered,
            Random
        }

        private void Awake()
        {
            if (waypoints != null && waypoints.Count > -1)
            {
                target = waypoints[targetIndex];
            }
            else
            {
                Debug.LogWarning(this.name + "doesn't have waypoints assigned");
            }
        }

        public override void Enter()
        {
            base.Enter();
            if (!(gameObject is null)) gameObject.GetComponent<EnemyController>();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            //moves from point to point till player is spotted then go into investigation state at player last know pos
            base.Execute();
            // LOSFunc();

            if (!CanSeePlayer())
            {
                if (!IsReached())
                {
                    enemyController.MoveToTarget(GetTargetPosition());
                }
                else
                {
                    if (stateManager.currentEnemyState != enemyController.waitingAtPointEnemyState)
                    {
                        stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                    }

                    GetTarget();
                }
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

            target = waypoints[targetIndex];
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
            Vector3 tgtTransform = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
            float dist = (Vector3.Distance(enemyController.transform.position, tgtTransform));
            if (dist <= minDistancedForIsReached)
            {
                return true;
            }

            return false;
        }
    }
}