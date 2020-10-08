using System;
using System.Collections;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class WaitingAtPointEnemyState : EnemyStateBase
    {
        public float waitTimer = 1.0f;
        public float maxLandRturn = 60;
        public float rotSpeed = 5;

        public override void Enter()
        {
            base.Enter();
            StartCoroutine(waitForSec());
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            //wait for so long, then either continue patrolling or
            base.Execute();
            enemyController.LookLeftAndRight();
        }

        IEnumerator waitForSec()
        {
            yield return new WaitForSeconds(waitTimer);

            if (!CanSeePlayer())
            {
                if (stateManager.previousEnemyState == enemyController.investigatingEnemyState)
                {
                    stateManager.ChangeState(enemyController.returningToPosEnemyState);
                }
                else if (stateManager.previousEnemyState == enemyController.patrollingEnemyState)
                {
                    stateManager.ChangeState(stateManager.previousEnemyState);
                }
                else if(stateManager.currentEnemyState == enemyController.spottingState)
                {
                    stateManager.ChangeState(stateManager.initialState);
                }
            }
            else
            {
                StartCoroutine(waitForSec());
            }
            
        }
    }
}