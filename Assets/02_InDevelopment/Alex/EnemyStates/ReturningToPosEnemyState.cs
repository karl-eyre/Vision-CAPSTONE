using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class ReturningToPosEnemyState : EnemyStateBase
    {
        [HideInInspector]
        public Vector3 target;


        public override void Enter()
        {
            base.Enter();

            if (enemyController.startingState.stayStationary)
            {
                target = enemyController.startingState.intialPos;
            }
            else
            {
                target = enemyController.posWhenInterrupted;
            }
        }

        public override void Exit()
        {
            base.Exit();
            lineOfSight.SoftResetLos();
            if (enemyController.beingDistracted)
            {
                Distracted();
            }
        }

        public override void Execute()
        {
            //should return to previous duty, so if they were patrolling return to patroll route, is stationary
            //return to stationary position
            base.Execute();

            enemyController.MoveToTarget(target);
            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                stateManager.ChangeState(enemyController.startingState);
                // stateManager.ChangeState(stateManager.interruptedState);
            }
        }
    }
}