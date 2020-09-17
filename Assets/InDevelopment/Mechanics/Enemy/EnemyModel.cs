using System.Collections;
using System.Collections.Generic;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using UnityEngine;

namespace InDevelopment.Mechanics.Enemy
{
    public class EnemyModel : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public List<GameObject> waypoints;

        public int currentIndex = 0;

        public float speed;
        private float WaypointRadius = 1;


        public float WaitTime;

        public float rotSpeed = 2f;
        public float maxRotation = 45f;

        [HideInInspector]
        public bool investigating;

        public bool waiting;

        [SerializeField]
        private bool stationary;

        [HideInInspector]
        public Vector3 targetLastKnownPos;

        public Vector3 targetPosition;
        public Vector3 previousPos;
        public Quaternion previousRot;

        public StateManager stateManager;
        public PlayerDetectedState playerDetectedState;
        public InvestigatingState InvestigatingState;
        // public enum States
        // {
        //     patrolling,
        //     investigating,
        //     stationary,
        //     waitingAtPoint,
        //     returningToPos,
        //     playerDetected
        // }
        //
        // [Header("Debug"), Tooltip("Leave ALONE")]
        // public States states = States.patrolling;

        // public States previousState;

        public LineOfSight.LineOfSight lineOfSight;

        public float investigationThreshold = 50f;
        public float maxDetection = 100f;

        private void Start()
        {
            previousPos = transform.position;
            previousRot = transform.rotation;
            lineOfSight = GetComponent<LineOfSight.LineOfSight>();
            currentIndex = 0;

            if (waypoints == null)
            {
                Debug.LogError("Add Waypoints to enemy waypoint list");
            }
            else
            {
                transform.position = waypoints[0].transform.position;
            }

            investigating = false;
            waiting = false;
        }

        private void FixedUpdate()
        {
            CheckWaypointIndex();
            DetectLossState();
        }

        public bool IsDetecting()
        {
            if (lineOfSight != null && lineOfSight.isDetecting)
            {
                targetLastKnownPos = lineOfSight.player.gameObject.transform.position;
                LookAt(lineOfSight.player.transform);
                return true;
            }

            return false;
        }

        private void LookAt(Transform target)
        {
            transform.LookAt(target.transform.position);
        }

        private bool InvestigationThresholdExceeded()
        {
            if (lineOfSight.detectionMeter >= investigationThreshold)
            {
                return true;
            }

            return false;
        }

        public void InvestigationTrigger()
        {
            //check if detection meter is above half way, if so then go into investigation state
            //pass in player gameobject referenced in line of sight script as target position
            //maybe move checks for is null and is detecting into here
            if (!InvestigationThresholdExceeded()) return;

            stateManager.ChangeState(InvestigatingState);
            lineOfSight.stopDecrease = true;
        }

        private void DetectLossState()
        {
            if (lineOfSight.detectionMeter >= maxDetection)
            {
                stateManager.ChangeState(playerDetectedState);
            }
        }

        // public void ChangeState(States newState)
        // {
        //     states = newState;
        // }

        public bool CheckDistance(Vector3 targetPos)
        {
            if (Vector3.Distance(targetPos, transform.position) < WaypointRadius)
            {
                return true;
            }

            return false;
        }

        public void LookLeftAndRight()
        {
            transform.rotation = Quaternion.Euler(0f, maxRotation * Mathf.Sin(Time.time * rotSpeed), 0f);
        }

        public void MoveToTarget(Vector3 target)
        {
            //TODO: Change to move by navmesh agent rather than transform position
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            Vector3 direction = (target - transform.position).normalized;

            if (!lineOfSight.isDetecting)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = lookRotation;
            }
            else
            {
                transform.LookAt(lineOfSight.player.transform.position);
            }
        }

        private void CheckWaypointIndex()
        {
            if (currentIndex >= waypoints.Count)
            {
                currentIndex = 0;
            }
        }
    }
}