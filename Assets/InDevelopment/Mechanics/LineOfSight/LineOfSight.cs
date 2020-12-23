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

        public PlayerDetectionUI PlayerDetectionUI;

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
        public bool alerted;

        private bool paused;

        private void Start()
        {
            //will always be calling this coroutine
            StartCoroutine(RaycastToPlayer(.20f));
            if (player == null)
            {
                if (!FindObjectOfType<EnemyTarget>())
                {
                    Debug.Log("No player exists in the scene");
                    return;
                }

                player = FindObjectOfType<EnemyTarget>().gameObject;
                PlayerDetectionUI = FindObjectOfType<PlayerDetectionUI>();
            }

            if (!(MenuManager.instance is null)) MenuManager.pauseGame += () => paused = !paused;
            PlayerDetectionUI = FindObjectOfType<PlayerDetectionUI>();
        }

        private void FixedUpdate()
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

            if (canSeePlayer && !PlayerDetectionUI.enemies.Contains(this))
            {
                PlayerDetectionUI.enemies.Add(this);
            }
            else if (!canSeePlayer && PlayerDetectionUI.enemies.Contains(this))
            {
                PlayerDetectionUI.enemies.Remove(this);
            }
        }

        public float DistToTarget()
        {
            return Vector3.Distance(player.transform.position, headPos.transform.position);
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

            //get direction to player
            Vector3 dirToTarget = (player.transform.position - headPos.transform.position);
            deltaTime = Time.timeSinceLevelLoad - timeSinceLastSeen;

            //ensure that the player is in the view angle
            if (Vector3.Angle(headPos.transform.forward, dirToTarget) < viewAngle / 2)
            {

                Ray ray = new Ray(headPos.transform.position, dirToTarget);

                //if it hits the player then obviously you can see the player
                if (Physics.Raycast(ray, out hitInfo, viewDistance))
                {
                    if (hitInfo.collider.CompareTag("Obstacles"))
                    {
                        canSeePlayer = false;
                        return;
                    }
                    
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        canSeePlayer = true;
                        return;
                    }
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
            //used for the handle drawing
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

        //Depreciated
        public void SoftResetLos()
        {
            if (!isResetting)
            {
                isResetting = true;
                StartCoroutine(SoftResetLOS());
            }
        }
        //Depreciated
        public void HardResetLos()
        {
            if (!isResetting)
            {
                isResetting = true;
                StartCoroutine(HardResetLOS());
            }
        }
        //Depreciated
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
        //Depreciated
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