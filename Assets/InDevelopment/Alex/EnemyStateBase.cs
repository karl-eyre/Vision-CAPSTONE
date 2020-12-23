using System;
using System.Collections;
using System.Threading;
using InDevelopment.Alex.EnemyStates;
using InDevelopment.Mechanics.LineOfSight;
using UnityEngine;

namespace InDevelopment.Alex
{
    public class EnemyStateBase : MonoBehaviour
    {
        /// <summary>
        /// Detect sounds here and have a universal function to switch to the investigating state.
        /// Figure out what stepts to take next when we get there.
        /// </summary>
        [HideInInspector]
        public EnemyController enemyController;

        [HideInInspector]
        public StateManager stateManager;

        [HideInInspector]
        public LineOfSight lineOfSight;

        [HideInInspector]
        public Vector3 lastKnownPlayerPosition;

        [HideInInspector]
        public bool isAlerted;

        private bool isResetting;
        public bool playerDetected;

        public Animator animator;
        public AnimationClip clip;
        public bool playingAnimation;

        private void Start()
        {
            Init();
            playerDetected = false;
        }

        private void Init()
        {
            if (stateManager == null)
            {
                stateManager = GetComponent<StateManager>();
            }

            if (lineOfSight == null)
            {
                lineOfSight = GetComponentInParent<LineOfSight>();
            }

            if (enemyController == null)
            {
                enemyController = GetComponentInParent<EnemyController>();
            }

            if (!(enemyController is null)) enemyController.beingDistracted = false;
        }

        public void OnEnable()
        {
            Init();
        }

        public virtual void Enter()
        {
            Init();
        }

        public virtual void Exit()
        {
            
        }

        public virtual void Execute()
        {
            
        }

        //update needs to be called as well as execute because this function needs to run all the time
        //however execute only runs while in a state, but during a state change that execute doesn't run
        public void Update()
        {
            CalledAllTheTime();
        }

        public void LOSFunc()
        {
            //only call function in each state manually it seems to be causing problems for currently
            //checks for what state the AI needs to change to if needed
            if (!(enemyController.playerDetectedState is null) &&
                stateManager.currentEnemyState != enemyController.playerDetectedState && !playerDetected)
            {
                if (CanSeePlayer() && stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                {
                    if (stateManager.currentEnemyState != enemyController.spottingState)
                    {
                        stateManager.ChangeState(enemyController.spottingState);
                    }
                }

                if (!(enemyController is null) && enemyController.beingDistracted)
                {
                    if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                    {
                        stateManager.ChangeState(enemyController.investigatingEnemyState);
                    }
                }

                if (!(enemyController is null) && !CanSeePlayer() && !enemyController.beingDistracted &&
                    !lineOfSight.detected)
                {
                    if (stateManager.previousEnemyState != enemyController.spottingState)
                    {
                        lineOfSight.stopDecrease = false;
                        // lineOfSight.HardResetLos();
                    }
                }
            }
        }

        private void CalledAllTheTime()
        {
            if (CanSeePlayer())
            {
                AssignPlayerPos();
            }

            IsAlerted();
            LOSFunc();
            PlayerDetected();
        }

        public void GetDistracted(Vector3 location)
        {
            //used by the objects that get thrown around
            if (enemyController.beingDistracted) return;

            if (stateManager.currentEnemyState != enemyController.spottingState)
            {
                Distracted();
                enemyController.investigatingEnemyState.lastKnownPlayerPosition = location;
                enemyController.posWhenInterrupted = transform.position;
                stateManager.interruptedState = stateManager.currentEnemyState;
                if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                {
                    stateManager.ChangeState(enemyController.investigatingEnemyState);
                }
            }
        }

        public IEnumerator HearSomethingAnimation(Vector3 location)
        {
            if (stateManager.currentEnemyState != enemyController.spottingState)
            {
                animator.SetBool("HeardSomething", true);
                yield return new WaitForSeconds(clip.length);
                animator.SetBool("HeardSomething", false);
                GetDistracted(location);
            }
        }

        public void Distracted()
        {
            enemyController.beingDistracted = !enemyController.beingDistracted;
        }

        public void LookAtPlayer()
        {
            enemyController.LookAtTarget(lineOfSight.player.transform.position);
        }

        private void PlayerDetected()
        {
            //is the player detected if so then change to player detected state
            if (lineOfSight.detectionMeter > lineOfSight.maxDetectionValue)
            {
                if (stateManager.currentEnemyState != enemyController.playerDetectedState)
                {
                    lineOfSight.detected = true;
                    stateManager.ChangeState(enemyController.playerDetectedState);
                    enemyController.agent.autoBraking = false;
                }
            }
        }

        public void ResetAIState()
        {
            lineOfSight.detected = false;
            lineOfSight.canSeePlayer = false;
            enemyController.agent.autoBraking = true;
            enemyController.agent.speed = enemyController.defaultMoveSpeed;
            lineOfSight.detectionMeter = 0f;
            if (stateManager.currentEnemyState != enemyController.startingState)
            {
                stateManager.ChangeState(enemyController.startingState);
            }
        }

        private void TriggerPlayerDetection()
        {
            if (stateManager.currentEnemyState != enemyController.playerDetectedState)
            {
                // stateManager.ChangeState(enemyController.playerDetectedState);
            }
        }

        public bool AboveInvestigationThresholdCheck()
        {
            return lineOfSight.detectionMeter >= lineOfSight.investigationThreshold;
        }

        private void IsAlerted()
        {
            if (lineOfSight.detectionMeter > 0 && CanSeePlayer())
            {
                isAlerted = true;
                lineOfSight.alerted = true;
                lineOfSight.stopDecrease = true;
            }
            else
            {
                isAlerted = false;
                lineOfSight.alerted = false;
            }
        }

        //needs to be called all the time
        public void AssignPlayerPos()
        {
            lastKnownPlayerPosition = lineOfSight.player.transform.position;
            // posWhenInterrupted = enemyController.transform.position;
        }

        public bool CanSeePlayer()
        {
            return !(lineOfSight is null) && lineOfSight.canSeePlayer;
        }
    }
}