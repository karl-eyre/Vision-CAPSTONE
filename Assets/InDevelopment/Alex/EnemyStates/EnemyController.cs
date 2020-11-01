using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            SetupStates();
            SetupNavmesh();
        }

        void SetupNavmesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.ResetPath();
            agent.isStopped = true;
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
            // stateManager.initialState = startingState;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                //TODO end game
                F_Music.music.setParameterByName("MusicState", 2f, false);
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        public void MoveToTarget(Vector3 tgt)
        {
            //TODO: find how to get closest nav mesh point when moving to target
            tgt = new Vector3(tgt.x,transform.position.y,tgt.z);

            if (!(agent is null) && (agent.isStopped || agent.remainingDistance < 0.5f))
            {
                agent.ResetPath();
            }

            if (!(agent is null))
            {
                agent.SetDestination(tgt);
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