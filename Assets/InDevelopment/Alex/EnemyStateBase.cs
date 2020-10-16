﻿using System;
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

        // public Vector3 posWhenInterrupted;
        [HideInInspector]
        public Vector3 lastKnownPlayerPosition;

        [HideInInspector]
        public bool isAlerted;


        private bool isResetting;

        // public bool beingDistracted;

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
            enemyController.beingDistracted = false;
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
            //TODO:make so that no states change if in player detected state
            
            //only call function in each state manually it seems to be causing problems for currently
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
            
            //TODO:reset in better place

            if (!CanSeePlayer() && !enemyController.beingDistracted)
            {
                if (stateManager.previousEnemyState != enemyController.spottingState)
                {
                    lineOfSight.SoftResetLos();
                }
            }
        }

        private void CalledAllTheTime()
        {
            if (CanSeePlayer())
            {
                IsAlerted();
                AssignPlayerPos();
            }

            LOSFunc();
            PlayerDetected();
        }

        public void GetDistracted(Vector3 location)
        {
            if(enemyController.beingDistracted) return;

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

        public void Distracted()
        {
            enemyController.beingDistracted = !enemyController.beingDistracted;
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
            return !(lineOfSight is null) && lineOfSight.canSeePlayer;
        }
    }
}