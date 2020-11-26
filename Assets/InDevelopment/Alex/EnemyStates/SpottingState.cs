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
        }

        public override void Exit()
        {
            base.Exit();
            // enemyPosWhenInterrupted = enemyController.posWhenInterrupted;
        }

        public override void Execute()
        {
            base.Execute();
            LookAtPlayer();
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
}