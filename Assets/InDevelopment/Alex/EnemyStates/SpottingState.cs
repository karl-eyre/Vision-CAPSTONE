using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class SpottingState : EnemyStateBase
    {
        //merely stands still and looks at player.

        public Vector3 enemyPosWhenInterrupted;
        public override void Enter()
        {
            base.Enter();
            stateManager.interruptedState = stateManager.previousEnemyState;
            posWhenInterrupted = transform.position;
        }

        public override void Exit()
        {
            base.Exit();
            enemyPosWhenInterrupted = posWhenInterrupted;
        }

        public override void Execute()
        {
            base.Execute();

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

            LookAtPlayer();
        }
    }
}