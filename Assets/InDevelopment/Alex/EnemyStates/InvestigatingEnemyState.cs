using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingEnemyState : EnemyStateBase
    {
        private Transform target;

        public override void Enter()
        {
            base.Enter();
            target = lastKnownPlayerPosition;
            Debug.Log("I am in the " + this.GetType() + " state.");
        }

        public override void Exit()
        {
            base.Exit();
            if (!lineOfSight.canSeePlayer)
            {
                lineOfSight.ResetLos();
            }
        }

        public override void Execute()
        {
            //Move to last know player position if you lose player otherwise slowing move towards while continuing to detect player
            //once player is lost wait at last know pos
            
            
            //LookLeftAndRight + waitAtPoint
            if (target)
            {
                if (lineOfSight.canSeePlayer)
                {
                    enemyController.MoveToTarget(target.position);
                }
                else
                {
                    stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }

                if (enemyController.patrollingEnemyState.IsReached())
                {
                    enemyController.LookLeftAndRight();

                    //stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }


                if (!lineOfSight.canSeePlayer)
                {
                    stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }
            }

            base.Execute();
        }
    }
}