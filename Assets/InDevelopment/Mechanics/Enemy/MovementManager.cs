using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace InDevelopment.Mechanics.Enemy
{
    public class MovementManager : MonoBehaviour
    {
        public List<Transform> waypoints = new List<Transform>();
        public float moveSpeed;
        private Transform targetPosition;

        private NavMeshAgent agent;

        [SerializeField]
        private int currentIndex = 0;

        public bool randomPatrol;
        public float distanceThreshold = 0.5f;

        private void Start()
        {
            currentIndex = 0;
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            agent.speed = moveSpeed;
            transform.position = waypoints[0].position;
            MoveToPoint(waypoints[currentIndex].position);
        }

        private void MoveToPoint(Vector3 targetPos)
        {
            if (waypoints.Count == 0) 
            {
                return;
            }

            if (randomPatrol)
            {
                RandomPoint();
                agent.SetDestination(targetPos);
            }
            else
            {
                agent.SetDestination(targetPos);
            }
        }

        private void ChangeIndex()
        {
            if (currentIndex >= waypoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
        }

        private int RandomPoint()
        {
            currentIndex = Random.Range(0, waypoints.Count);
            return currentIndex;
        }

        private bool CheckDistance()
        {
            return agent.remainingDistance < distanceThreshold && !agent.pathPending;
        }

        private bool Investigating()
        {
            //add in actual investigation check when state machine is finished
            return false;
        }

        private bool Waiting()
        {
            //add in actual waiting check when state machine is finished
            return false;
        }

        private void Patrol()
        {
            if (CheckDistance() && !Waiting())
            {
                ChangeIndex();
                MoveToPoint(waypoints[currentIndex].position);
                transform.LookAt(waypoints[currentIndex].position);
            }
        }
        
        private void Update()
        {
            //finished when state machine is completed
            if (!Investigating() && targetPosition != null)
            {
                Patrol();
            }
            else
            {
                targetPosition = waypoints[0].transform;
                MoveToPoint(targetPosition.position);
            }
        }
    }
}