using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Mechanics.Enemy;
using InDevelopment.Mechanics.Menu;
using InDevelopment.Mechanics.Player;
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
        public float investigationThreshold = 50f;
        public float maxDetectionValue = 100f;


        private RaycastHit hitInfo;
        private float timeSinceLastSeen;
        private bool inRange;
        private float deltaTime;

        public Transform headPos;


        [HideInInspector]
        public bool stopDecrease;

        public float resetDelay = 2f;
        private bool isResetting = false;
        public bool detected;

        private bool paused;

        private void Start()
        {
            StartCoroutine(RaycastToPlayer(.25f));
            //perhaps find a better way to assign player
            if (player == null)
            {
                if (!FindObjectOfType<EnemyTarget>())
                {
                    Debug.Log("No player exists in the scene");
                    return;
                }

                player = FindObjectOfType<EnemyTarget>().gameObject;
            }
            MenuManager.instance.pauseGame += () =>  paused = !paused;
            
        }

        private void Update()
        {
            if (!paused)
            {
                if (!canSeePlayer && !stopDecrease && !detected)
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

                if (canSeePlayer)
                {
                    detectionMeter += fillSpeed * deltaTime;
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
                angleInDegrees += headPos.transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public void SoundDistraction(float noiseLoudness)
        {
            // detectionMeter += noiseLoudness;
            stopDecrease = true;
        }

        public void SoftResetLos()
        {
            if (!isResetting)
            {
                isResetting = true;
                StartCoroutine(SoftResetLOS());
            }
        }

        public void HardResetLos()
        {
            if (!isResetting)
            {
                isResetting = true;
                StartCoroutine(HardResetLOS());
            }
        }

        private IEnumerator SoftResetLOS()
        {
            yield return new WaitForSeconds(resetDelay);
            if (!canSeePlayer)
            {
                if (detectionMeter >= investigationThreshold)
                {
                    detectionMeter = investigationThreshold - 5;
                }
                else
                {
                    detectionMeter = detectionMeter;
                }

                stopDecrease = false;
            }

            isResetting = false;
        }

        private IEnumerator HardResetLOS()
        {
            yield return new WaitForSeconds(resetDelay);
            if (!canSeePlayer)
            {
                detectionMeter = 0;
                stopDecrease = false;
            }

            isResetting = false;
        }
    }
}