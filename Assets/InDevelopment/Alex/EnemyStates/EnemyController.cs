using System;
using InDevelopment.Mechanics.LineOfSight;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace InDevelopment.Alex.EnemyStates
{
    public class EnemyController : MonoBehaviour
    {
        /// <summary>
        /// What do i want this enemy to do?
        /// Patrol, Investigate, Stationary, WaitingAtPoint, ReturnToPos, DetectPlayer
        ///
        /// Detect and handle sound.
        /// </summary>

        #region Variables

        public float rightTurnRadius = 45f;

        public float leftTurnRadius = -45f;
        public float rotSpeed = 5;
        public float moveSpeed;

        [Header("=Debug=")]
        public Quaternion rotation;

        public Vector3 direction;

        F_Occlusion searchingSound;

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

        private LineOfSight lineOfSight;

        public Animator animator;

        private float moveSpeedX;
        private float moveSpeedZ;

        private int moveSpeedHash;
        private float agentVelocity;
        private float rotationAmount;

        #endregion


        // Start is called before the first frame update
        private void Start()
        {
            searchingSound = GetComponent<F_Occlusion>();
            agent = GetComponent<NavMeshAgent>();
            lineOfSight = GetComponent<LineOfSight>();
            moveSpeedHash = Animator.StringToHash("MoveSpeed");
            SetupStates();
            SetupNavmesh();
        }

        void SetupNavmesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.ResetPath();
            // agent.isStopped = true;
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

        private void Update()
        {
            UpdateAnimator();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                //TODO end game
                F_Music.music.setParameterByName("MusicState", 2f, false);
                searchingSound.searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        private void UpdateAnimator()
        {
            if (stateManager.currentEnemyState != playerDetectedState)
            {
                agentVelocity = agent.velocity.magnitude / agent.speed;
            }
            else
            {
                agentVelocity = 2f;
            }

            animator.SetFloat(moveSpeedHash, agentVelocity);
        }

        public void MoveToTarget(Vector3 tgt)
        {
            tgt = new Vector3(tgt.x, transform.position.y, tgt.z);

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
            // lineOfSight.headPos.transform.LookAt(tgt);
            lineOfSight.headPos.transform.LookAt(tgt, Vector3.up);
        }

        public void LookLeftAndRight()
        {
            //change to move til it hits max and min angles
            // float rotationAmount = Mathf.SmoothStep(lineOfSight.headPos.transform.rotation.y + turnRadius,lineOfSight.headPos.transform.rotation.y - turnRadius,Mathf.PingPong(Time.time * rotSpeed,1));
            if (lineOfSight.headPos.transform.rotation.y <= rightTurnRadius)
            {
                rotationAmount = Mathf.SmoothStep(transform.rotation.y, transform.rotation.y + leftTurnRadius,
                    Time.time * rotSpeed);
            }
            else if (lineOfSight.headPos.transform.rotation.y >= leftTurnRadius)
            {
                rotationAmount = Mathf.SmoothStep(transform.rotation.y, transform.rotation.y + rightTurnRadius,
                    Time.time * rotSpeed );
            }

            lineOfSight.headPos.transform.Rotate(0, rotationAmount * Time.deltaTime, 0, Space.Self);
        }
    }
}