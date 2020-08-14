using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace InDevelopment.Mechanics.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
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
        public Vector3 playerLastKnownPos;

        private Vector3 targetPosition;
        private Vector3 stationaryPos;
        private Quaternion stationaryRot;

        private enum states
        {
            patrolling,
            investigating,
            stationary,
            waiting
        }

        private void Start()
        {
            stationaryPos = transform.position;
            stationaryRot = transform.rotation;
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

        private void Update()
        {
            


            CheckWaypointIndex();
            //TODO make it so that if the target is stationary then after investigating go back to original stationaryPos
            if (!investigating)
            {
                targetPosition = waypoints[currentIndex].transform.position;
                MoveToTarget(targetPosition);
            }
            else
            {
                MoveToTarget(playerLastKnownPos);
            }

            if (stationary)
            {
                LookLeftAndRight();
                targetPosition = stationaryPos;
            }


            if (Vector3.Distance(waypoints[currentIndex].transform.position, transform.position) < WaypointRadius &&
                !stationary)
            {
                if (!waiting)
                {
                    StartCoroutine(WaitAtWaypoint());
                }
            }

            if (Vector3.Distance(playerLastKnownPos, transform.position) < WaypointRadius && !stationary)
            {
                StartCoroutine(WaitAtInvestigatePoint());
            }

            if (Vector3.Distance(stationaryPos, transform.position) < WaypointRadius)
            {
                // stationary = true;
                // transform.position = stationaryPos;
                // transform.rotation = stationaryRot;
            }
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

        IEnumerator WaitAtInvestigatePoint()
        {
            yield return new WaitForSeconds(WaitTime);
            //just to ensure that the player can't accidently end up near the last know position by accident,
            //since you can't null out a vector 3
            playerLastKnownPos = new Vector3(9999999, 99999999, 9999999);
            investigating = false;
        }

        IEnumerator WaitAtWaypoint()
        {
            waiting = true;
            yield return new WaitForSeconds(WaitTime);
            waiting = false;
            if (investigating)
            {
                yield break;
            }
            currentIndex++;
        }
    }
}