using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using InDevelopment.Mechanics.LineOfSight;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InDevelopment.Mechanics.Enemy
{
    public class EnemyStateMachine : MonoBehaviour
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
            returningToPos,
            playerDetected
        }

        [Header("Debug"), Tooltip("Leave ALONE")]
        public States states = States.patrolling;

        private States previousState;

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

            states = States.patrolling;
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
            DetectLossState();

            switch (states)
            {
                case States.patrolling:

                    if (IsDetecting())
                    {
                        DetectingPlayer();
                    }
                    else
                    {
                        if (waypoints != null) targetPosition = waypoints[currentIndex].transform.position;

                        MoveToTarget(targetPosition);

                        if (CheckDistance(targetPosition))
                        {
                            previousState = States.patrolling;
                            previousPos = transform.position;
                            ChangeState(States.waitingAtPoint);
                        }
                    }

                    break;

                case States.investigating:
                    //figure out how to get investigation threshold to work
                    if (IsDetecting())
                    {
                        DetectingPlayer();
                    }
                    else
                    {
                        investigating = true;
                        MoveToTarget(targetLastKnownPos);
                        if (CheckDistance(targetLastKnownPos))
                        {
                            ChangeState(States.waitingAtPoint);
                        }
                    }

                    break;

                case States.stationary:

                    if (IsDetecting())
                    {
                        DetectingPlayer();
                        return;
                    }
                    else
                    {
                        transform.position = previousPos;
                        transform.rotation = previousRot;
                        LookLeftAndRight();
                    }

                    //put in if statement that checks are you in your original spot if not move there
                    previousState = States.stationary;

                    break;

                case States.waitingAtPoint:

                    if (IsDetecting())
                    {
                        DetectingPlayer();
                    }

                    if (!waiting)
                    {
                        StartCoroutine(WaitAtWaypoint());
                    }


                    break;

                case States.returningToPos:

                    if (IsDetecting())
                    {
                        DetectingPlayer();
                    }
                    else
                    {
                        DetectingPlayer();
                        MoveToTarget(previousPos);
                        if (CheckDistance(previousPos))
                        {
                            states = previousState;
                        }
                    }

                    break;

                case States.playerDetected:
                    Debug.Log("player loses");
                    // Scene scene = SceneManager.GetActiveScene();
                    // SceneManager.LoadScene(scene.buildIndex);
                    break;
            }
        }

        public bool IsDetecting()
        {
            if (lineOfSight != null && lineOfSight.isDetecting)
            {
                transform.LookAt(lineOfSight.player.transform.position);
                return true;
            }

            return false;
        }

        public void DetectingPlayer()
        {
            //check if detection meter is above half way, if so then go into investigation state
            //pass in player gameobject referenced in line of sight script as target position
            //maybe move checks for is null and is detecting into here
            if (lineOfSight.detectionMeter >= investigationThreshold)
            {
                targetLastKnownPos = lineOfSight.player.gameObject.transform.position;
                lineOfSight.stopDecrease = true;
                Debug.Log("Investigating");
                ChangeState(States.investigating);
            }
        }

        public void DetectLossState()
        {
            if (lineOfSight.detectionMeter >= maxDetection)
            {
                ChangeState(States.playerDetected);
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

        IEnumerator WaitAtWaypoint()
        {
            waiting = true;
            yield return new WaitForSeconds(WaitTime);
            waiting = false;
            if (IsDetecting())
            {
                ChangeState(States.waitingAtPoint);
            }

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