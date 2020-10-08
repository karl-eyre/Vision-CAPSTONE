using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class SpottingState : EnemyStateBase
    {
        //merely stands still and looks at player.
        public override void Enter()
        {
            base.Enter();
            //TODO:figure out why pos when interrupted is not being assigned
            stateManager.interruptedState = stateManager.previousEnemyState;
            posWhenInterrupted = transform.position;
        }

        public override void Exit()
        {
            base.Exit();
            posWhenInterrupted = enemyController.transform.position;
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