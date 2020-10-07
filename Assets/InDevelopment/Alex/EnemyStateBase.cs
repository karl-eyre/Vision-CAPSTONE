using System;
using System.Collections;
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
        public Vector3 posWhenInterrupted;

        [HideInInspector]
        public Vector3 lastKnownPlayerPosition;

        [HideInInspector]
        public bool isAlerted;


        private bool isResetting;

        private void Init()
        {
            if (!stateManager)
            {
                stateManager = GetComponent<StateManager>();
            }

            if (!lineOfSight)
            {
                lineOfSight = GetComponentInParent<LineOfSight>();
            }

            if (!enemyController)
            {
                enemyController = GetComponentInParent<EnemyController>();
            }
        }

        public void Awake()
        {
            Init();
        }

        public virtual void Enter()
        {
            Init();
            Debug.Log("I have entered into the " + this.GetType() + " state.");
        }

        public virtual void Exit()
        {
            Debug.Log("I have exited into the " + this.GetType() + " state.");
        }

        public virtual void Execute()
        {
            IsAlerted();
            Debug.Log("I am executing the " + this.GetType() + " state.");
        }

        //call in each state when required
        //look at moving into line of sight script and just calling it all the time,
        //that would remove the state issue i believe
        public void LOSFunc()
        {
            if (PlayerDetected())
            {
                stateManager.ChangeState(enemyController.playerDetectedState);
                return;
            }

            //only call function in each state manually it seems to be causing problems for currently
            if (lineOfSight.canSeePlayer && isAlerted)
            {
                lastKnownPlayerPosition = lineOfSight.player.transform.position;

                if (AboveInvestigationThresholdCheck())
                {
                    if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                    {
                        stateManager.ChangeState(enemyController.investigatingEnemyState);
                    }
                }
                else if (stateManager.currentEnemyState != enemyController.spottingState)
                {
                    stateManager.ChangeState(enemyController.spottingState);
                }
            }
            else
            {
                if (isAlerted && stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                {
                    lineOfSight.HardResetLos();
                }
                else if (stateManager.currentEnemyState == enemyController.spottingState && !AboveInvestigationThresholdCheck())
                {
                    stateManager.ChangeState(stateManager.previousEnemyState);
                }
            }
        }

        public void LookAtPlayer()
        {
            enemyController.LookAtTarget(lineOfSight.player.transform.position);
        }

        private bool PlayerDetected()
        {
            return lineOfSight.detectionMeter > lineOfSight.maxDetectionValue;
        }

        private bool AboveInvestigationThresholdCheck()
        {
            return lineOfSight.detectionMeter >= lineOfSight.investigationThreshold;
        }

        private void IsAlerted()
        {
            if (lineOfSight.detectionMeter > 0)
            {
                isAlerted = true;
                lineOfSight.stopDecrease = true;
            }
            else
            {
                isAlerted = false;
            }
        }

        public void AssignPlayerPos()
        {
            lastKnownPlayerPosition = lineOfSight.player.transform.position;
        }

        public bool CanSeePlayer()
        {
            return lineOfSight.canSeePlayer;
        }
    }
}