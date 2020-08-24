using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Mechanics.Player;
using Player;
using UnityEngine;

namespace InDevelopment.Mechanics.LineOfSight
{
    public class LineOfSight : MonoBehaviour
    {
        [Header("Line of Sight Settings")]
        public float viewDistance;

        [Range(0, 360)]
        public float viewAngle;

        public LayerMask obstacleMask;
        private bool inRange;
        public float detectionMeter;
        public float fillSpeed;
        public float reduceSpeed;
        

        [Header("Other Settings")]
        public GameObject player;
        private float timeSinceLastSeen;
        public LayerMask playerMask;
        public bool isDetecting;

        private RaycastHit hitInfo;

        private float deltaTime;
        
        
        [HideInInspector]
        public bool stopDecrease;

        private void Start()
        {
            StartCoroutine(RaycastToPlayer(.25f));
            //perhaps find a better way to assign player
            if (player == null)
            {
                player = FindObjectOfType<PlayerMovementRb>().gameObject;
            }
        }

        private void Update()
        {
            if (!isDetecting && !stopDecrease)
            {
                if (detectionMeter > 0)
                {
                    detectionMeter -= reduceSpeed * deltaTime;
                }
                else
                {
                    detectionMeter = 0;
                }
            }
        }

        public float DistToTarget()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        public float ViewDistance()
        {
            return viewDistance;
        }

        IEnumerator RaycastToPlayer(float delay)
        {
            while (true)
            {
                CanSeePlayer();
                timeSinceLastSeen = Time.timeSinceLevelLoad;
                yield return new WaitForSeconds(delay);
            }
        }

        private void CanSeePlayer()
        {
            if (player == null) return;

            Vector3 dirToTarget = (player.transform.position - transform.position);
            deltaTime = Time.timeSinceLevelLoad - timeSinceLastSeen;
            
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                Ray ray = new Ray(transform.position, dirToTarget);

                if (Physics.Raycast(ray, out hitInfo, viewDistance, obstacleMask) ||
                    Vector3.Distance(player.transform.position, transform.position) > viewDistance)
                {
                    // Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1);
                    isDetecting = false;
                    return;
                }

                //figure out why raycast is going through walls
                if (Physics.Raycast(ray, out hitInfo, viewDistance, playerMask))
                {
                    // Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1);
                    isDetecting = true;
                    //scale detection meter by time since last seen
                    //not being filled when investigating
                    detectionMeter += fillSpeed * deltaTime;
                }
            }
            else
            {
                isDetecting = false;
                stopDecrease = false;
            }
        }
        
        //used by editor script
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}