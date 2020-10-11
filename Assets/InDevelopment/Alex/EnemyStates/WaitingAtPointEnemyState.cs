using System;
using System.Collections;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class WaitingAtPointEnemyState : EnemyStateBase
    {
        public float waitTimer = 1.0f;
        private bool waiting;

        public override void Enter()
        {
            base.Enter();
            if (!waiting)
            {
                waiting = true;
                StartCoroutine(waitForSec());
            }
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


            if (!waiting)
            {
                if (enemyController.beingDistracted)
                {
                    if (stateManager.currentEnemyState != stateManager.interruptedState)
                    {
                        stateManager.ChangeState(stateManager.interruptedState);
                    }
                }
                else
                {
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
                        else if (stateManager.previousEnemyState == enemyController.spottingState)
                        {
                            stateManager.ChangeState(enemyController.patrollingEnemyState);
                        }
                    }
                }
            }
        }

        IEnumerator waitForSec()
        {
            yield return new WaitForSeconds(waitTimer);

            waiting = false;
        }
    }
}