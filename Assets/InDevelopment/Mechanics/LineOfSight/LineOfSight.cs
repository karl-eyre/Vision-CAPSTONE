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
        /// <summary>
        /// For the most part nothing needs to get referenced from this script except variables.
        /// </summary>
        
        
        [Header("Line of Sight Settings")]
        public float viewDistance = 10f;
        public LayerMask playerMask;

        [Range(0, 360)]
        public float viewAngle = 100f;

        public LayerMask obstacleMask;
        public float fillSpeed = 10;
        public float reduceSpeed = 0.1f;
        

        [Header("Referenced Variables")]
        public GameObject player;
        public bool canSeePlayer;
        public float detectionMeter;
        public float investigationThreshold = 50;
        
        
        
        private RaycastHit hitInfo;
        private float timeSinceLastSeen;
        private bool inRange;
        private float deltaTime;
        
        
        [HideInInspector]
        public bool stopDecrease;

        public float resetDelay = 2f;
        private bool isResetting;

        private void Start()
        {
            StartCoroutine(RaycastToPlayer(.25f));
            //perhaps find a better way to assign player
            if (player == null)
            {
                if (!FindObjectOfType<PlayerMovement>())
                {
                    Debug.Log("No player exists in the scene");
                    return;
                }
                player = FindObjectOfType<PlayerMovement>().gameObject;
            }
        }

        private void Update()
        {
            if (!canSeePlayer && !stopDecrease)
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
            //Yes, We get it! (Leave this alone just because!)
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

                //This is for obstacles getting in the way.
                if (Physics.Raycast(ray, out hitInfo, viewDistance, obstacleMask) ||
                    Vector3.Distance(player.transform.position, transform.position) > viewDistance)
                {
                    // Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1);
                    canSeePlayer = false;
                    return;
                }
                
                //This is where the actual player detection happens
                if (Physics.Raycast(ray, out hitInfo, viewDistance, playerMask))
                {
                    // Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1);
                    canSeePlayer = true;
                    //scale detection meter by time since last seen
                    //not being filled when investigating
                    detectionMeter += fillSpeed * deltaTime;
                }
            }
            else
            {
                canSeePlayer = false;
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

        public void ResetLos()
        {
            if (!isResetting)
            {
                isResetting = true;
                StartCoroutine(ResetLOS());
            }
            
        }

        private IEnumerator ResetLOS()
        {
            yield return new WaitForSeconds(resetDelay);
            detectionMeter = investigationThreshold - 5;
            stopDecrease = false;
            isResetting = false;
        }
    }
}