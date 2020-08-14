using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace InDevelopment.Mechanics.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public List<GameObject> waypoints;
        private int currentIndex = 0;

        public float speed;
        private float WaypointRadius = 1;


        public float WaitTime;

        public float rotSpeed = 2f;
        public float maxRotation = 45f;

        [HideInInspector]
        public bool investigating;

        private bool waiting;

        [SerializeField]
        private bool stationary;

        [HideInInspector]
        public Vector3 targetLastKnownPos;

        private Vector3 targetPosition;
        private Vector3 previousPos;
        private Quaternion previousRot;

        public enum States
        {
            patrolling,
            investigating,
            stationary,
            waitingAtPoint,
            returningToPos
        }

        [Header("Debug"),Tooltip("Leave ALONE")]
        public States states = States.patrolling;
        private States previousState;

        private void Start()
        {
            previousPos = transform.position;
            previousRot = transform.rotation;
            currentIndex = 0;

            if (waypoints == null)
            {
                Debug.LogError("Add Waypoints to enemy waypoint list");
            }
            else
            {
                transform.position = waypoints[0].transform.position;
            }

            if (stationary)
            {
                states = States.stationary;
            }

            investigating = false;
            waiting = false;
        }

        private void Update()
        {
            CheckWaypointIndex();

            switch (states)
            {
                case States.patrolling:
                    targetPosition = waypoints[currentIndex].transform.position;
                    
                    MoveToTarget(targetPosition);

                    if (CheckDistance(targetPosition))
                    {
                        previousState = States.patrolling;
                        previousPos = transform.position;
                        ChangeState(States.waitingAtPoint);
                    }
                    break;

                case States.investigating:
                    investigating = true;
                    MoveToTarget(targetLastKnownPos);

                    if (CheckDistance(targetLastKnownPos))
                    {
                        ChangeState(States.waitingAtPoint);
                    }
                    break;

                case States.stationary:
                    //put in if statement that checks are you in your original spot if not move there
                    transform.position = previousPos;
                    transform.rotation = previousRot; 
                    LookLeftAndRight();
                    previousState = States.stationary;
                    break;

                case States.waitingAtPoint:
                    if (!waiting)
                    {
                        StartCoroutine(WaitAtWaypoint());
                    }
                    break;

                case States.returningToPos:
                    MoveToTarget(previousPos);
                    if (CheckDistance(previousPos))
                    {
                        states = previousState;
                    }
                    break;
            }
        }

        public void ChangeState(States newState)
        {
            states = newState;
        }

        private bool CheckDistance(Vector3 targetPos)
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

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }

        private void CheckWaypointIndex()
        {
            if (currentIndex >= waypoints.Count)
            {
                currentIndex = 0;
            }
        }

        IEnumerator WaitAtWaypoint()
        {
            waiting = true;
            yield return new WaitForSeconds(WaitTime);
            waiting = false;
            if (investigating)
            {
                targetLastKnownPos = new Vector3(9999999, 99999999, 9999999);
                investigating = false;
                ChangeState(States.returningToPos);
                yield break;
            }
            currentIndex++;
            ChangeState(States.returningToPos);
        }
    }
}