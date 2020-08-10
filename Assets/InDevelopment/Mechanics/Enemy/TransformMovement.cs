﻿using System;
using UnityEngine;

namespace InDevelopment.Mechanics.Enemy
{
    public class TransformMovement : MonoBehaviour
    {
        public GameObject[] waypoints;
        private int currentIndex = 0;
        private float rotSpeed;
        public float speed;
        private float WaypointRadius = 1;

        private void Start()
        {
            currentIndex = 0;
            transform.position = waypoints[0].transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(waypoints[currentIndex].transform.position, transform.position) < WaypointRadius)
            {
                currentIndex++;
                if (currentIndex >= waypoints.Length)
                {
                    currentIndex = 0;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndex].transform.position,
                Time.deltaTime * speed);
            Vector3 direction = (waypoints[currentIndex].transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
    }
}