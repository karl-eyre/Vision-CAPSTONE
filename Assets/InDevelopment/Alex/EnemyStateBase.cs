﻿using System;
using System.Collections;
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


        public Vector3 posWhenInterrupted;

        public Vector3 lastKnownPlayerPosition;

        [HideInInspector]
        public bool isAlerted;


        private bool isResetting;

        public bool beingDistracted;

        private void Init()
        {
            beingDistracted = false;
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
            Debug.Log("I am executing the " + this.GetType() + " state.");
        }

        public void Update()
        {
            CalledAllTheTime();
        }

        //look at moving into line of sight script and just calling it all the time
        //that would remove the state issue i believe
        public void LOSFunc()
        {
            //only call function in each state manually it seems to be causing problems for currently
            if (CanSeePlayer() && stateManager.currentEnemyState != enemyController.investigatingEnemyState)
            {
                AssignPlayerPos();

                if (stateManager.currentEnemyState != enemyController.spottingState)
                {
                    stateManager.ChangeState(enemyController.spottingState);
                }
            }

            if (beingDistracted)
            {
                if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                {
                    stateManager.ChangeState(enemyController.investigatingEnemyState);
                }
            }

            if (!CanSeePlayer() && !beingDistracted)
            {
                //TODO:find good place for reset
                if (stateManager.previousEnemyState != enemyController.spottingState)
                {
                    lineOfSight.HardResetLos();
                }
            }
        }

        private void CalledAllTheTime()
        {
            if (CanSeePlayer())
            {
                IsAlerted();
            }

            LOSFunc();
            PlayerDetected();
        }

        public void GetDistracted(Vector3 location)
        {
            if (stateManager.currentEnemyState != enemyController.spottingState)
            {
                Distracted();
                enemyController.investigatingEnemyState.lastKnownPlayerPosition = location;
                enemyController.returningToPosEnemyState.posWhenInterrupted = transform.position;
                if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                {
                    stateManager.ChangeState(enemyController.investigatingEnemyState);
                }
            }
        }

        public void Distracted()
        {
            beingDistracted = !beingDistracted;
        }

        //called when wanting to look at player
        public void LookAtPlayer()
        {
            enemyController.LookAtTarget(lineOfSight.player.transform.position);
        }

        //needs to be called all the time
        private void PlayerDetected()
        {
            if (lineOfSight.detectionMeter > lineOfSight.maxDetectionValue)
            {
                if (stateManager.currentEnemyState != enemyController.playerDetectedState)
                {
                    stateManager.ChangeState(enemyController.playerDetectedState);
                    return;
                }
            }
        }

        public bool AboveInvestigationThresholdCheck()
        {
            return lineOfSight.detectionMeter >= lineOfSight.investigationThreshold;
        }

        //needs to be called all the time
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

        //needs to be called all the time
        public void AssignPlayerPos()
        {
            lastKnownPlayerPosition = lineOfSight.player.transform.position;
            // posWhenInterrupted = enemyController.transform.position;
        }

        public bool CanSeePlayer()
        {
            return lineOfSight.canSeePlayer;
        }
    }
}