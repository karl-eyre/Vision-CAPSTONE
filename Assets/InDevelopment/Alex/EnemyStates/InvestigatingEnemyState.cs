using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingEnemyState : EnemyStateBase
    {
        public Vector3 target;

        public override void Enter()
        {
            base.Enter();
            target = lastKnownPlayerPosition;
            Debug.Log("I am in the " + this.GetType() + " state.");
        }

        public override void Exit()
        {
            base.Exit();
            lineOfSight.SoftResetLos();
            
        }

        public override void Execute()
        {
            base.Execute();
            //Move to last know player position if you lose player otherwise slowing move towards while continuing to detect player
            //once player is lost wait at last know pos
            if (CanSeePlayer())
            {
                target = lastKnownPlayerPosition;
                enemyController.MoveToTarget(target);
            }
            else
            {
                enemyController.MoveToTarget(target);
                if (Vector3.Distance(transform.position, target) < 0.5f && stateManager.currentEnemyState != enemyController.waitingAtPointEnemyState)
                {
                    stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }
            }
        }
    }
}