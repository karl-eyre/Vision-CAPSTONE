using System;
using UnityEngine;
using UnityEngine.AI;

namespace InDevelopment.Alex.EnemyStates
{
    public class EnemyController : MonoBehaviour
    {
        /// <summary>
        /// What do i want this enemy to do?
        /// Patrol, Investigate, Stationary,, WaitingAtPoint, ReturnToPos, DetectPlayer
        ///
        /// Detect and handle sound.
        /// </summary>

        #region Variables

        public float turnRadius = 60;

        public float rotSpeed = 5;
        public float moveSpeed;

        [Header("=Debug=")]
        public Quaternion rotation;

        public Vector3 direction;


        [HideInInspector]
        public StateManager stateManager;

        private EnemyStateBase _enemyState;

        [HideInInspector]
        public bool beingDistracted;

        [HideInInspector]
        public Vector3 posWhenInterrupted;

        [HideInInspector]
        public NavMeshAgent agent;

        //[Header("States")]
        [HideInInspector]
        public PatrollingEnemyState patrollingEnemyState;

        [HideInInspector]
        public StationaryEnemyState stationaryEnemyState;

        [HideInInspector]
        public WaitingAtPointEnemyState waitingAtPointEnemyState;

        [HideInInspector]
        public ReturningToPosEnemyState returningToPosEnemyState;

        [HideInInspector]
        public InvestigatingEnemyState investigatingEnemyState;

        [HideInInspector]
        public PlayerDetectedState playerDetectedState;

        [HideInInspector]
        public StartingState startingState;

        [HideInInspector]
        public SpottingState spottingState;

        #endregion

        // Start is called before the first frame update
        private void Awake()
        {
            SetupStates();
            SetupNavmesh();
        }

        void SetupNavmesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = true;
            agent.speed = moveSpeed;
        }

        void SetupStates()
        {
            stateManager = GetComponentInChildren<StateManager>();
            patrollingEnemyState = GetComponentInChildren<PatrollingEnemyState>();
            stationaryEnemyState = GetComponentInChildren<StationaryEnemyState>();
            waitingAtPointEnemyState = GetComponentInChildren<WaitingAtPointEnemyState>();
            returningToPosEnemyState = GetComponentInChildren<ReturningToPosEnemyState>();
            investigatingEnemyState = GetComponentInChildren<InvestigatingEnemyState>();
            playerDetectedState = GetComponentInChildren<PlayerDetectedState>();
            startingState = GetComponentInChildren<StartingState>();
            spottingState = GetComponentInChildren<SpottingState>();
            stateManager.ChangeState(startingState);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                //TODO end game
            }
        }

        public void MoveToTarget(Vector3 tgt)
        {
            //TODO: use this if fix for object being thrown at wall doesn't work
            // tgt = new Vector3(tgt.x,transform.position.y,tgt.z);

            if (agent.isStopped || agent.remainingDistance < 0.5f)
            {
                agent.ResetPath();
            }

            agent.SetDestination(tgt);

            if (agent.pathPending)
            {
                if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(tgt);
                }

                if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    agent.ResetPath();
                }

                if (agent.pathStatus == NavMeshPathStatus.PathPartial)
                {
                    NavMeshHit hit;
                    agent.FindClosestEdge(out hit);
                    agent.SetDestination(hit.position);
                }
            }
        }

        public void LookAtTarget(Vector3 tgt)
        {
            transform.LookAt(tgt);
        }

        public void LookLeftAndRight()
        {
            var t = transform.position;
            transform.rotation = Quaternion.Euler(0f, turnRadius * Mathf.Sin(Time.time * rotSpeed), 0f);
            //TODO: Fix this so that it does the look left and right thing based on its current looking direction!
        }
    }
}