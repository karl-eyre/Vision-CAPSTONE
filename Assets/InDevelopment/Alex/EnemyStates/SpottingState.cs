using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class SpottingState : EnemyStateBase
    {
        //merely stands still and looks at player.

        // [HideInInspector]
        // public Vector3 enemyPosWhenInterrupted;
        public override void Enter()
        {
            base.Enter();
            enemyController.agent.ResetPath();
            stateManager.interruptedState = stateManager.previousEnemyState;
            enemyController.posWhenInterrupted = transform.position;
            if (stateManager.previousEnemyState != enemyController.playerDetectedState)
            {
                StartCoroutine(PlayAnimation());
            }
        }

        public override void Exit()
        {
            base.Exit();
            // enemyPosWhenInterrupted = enemyController.posWhenInterrupted;
            lineOfSight.headPos.rotation = Quaternion.Euler(0, 0, 0);
        }

        public override void Execute()
        {
            base.Execute();
            // LookAtPlayer();
            if (!enemyController.stateManager.currentEnemyState.playingAnimation)
            {
                if (AboveInvestigationThresholdCheck())
                {
                    if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
                    {
                        stateManager.ChangeState(enemyController.investigatingEnemyState);
                    }
                }
                else
                {
                    if (!CanSeePlayer())
                    {
                        stateManager.ChangeState(stateManager.interruptedState);
                    }
                }
            }
        }

        
        
        private IEnumerator PlayAnimation()
        {
            enemyController.spottingState.playingAnimation = true;
            enemyController.spottingState.animator.SetBool("SawSomething", true);
            yield return new WaitForSeconds(enemyController.spottingState.clip.length);
            enemyController.spottingState.animator.SetBool("SawSomething", false);
            enemyController.spottingState.playingAnimation = false;
        }
    }
}